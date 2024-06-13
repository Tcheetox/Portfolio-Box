using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Portfolio_Box.Models;
using Portfolio_Box.Models.Files;
using Portfolio_Box.Models.Users;
using Portfolio_Box.Utilities;
using static System.Collections.Specialized.BitVector32;

namespace Portfolio_Box.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileRepository _fileRepository;
        private readonly IFileFactory _fileFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FileController(
            ILogger<FileController> logger,
            IFileRepository fileRepository,
            IFileFactory fileFactory,
            IConfiguration configuration)
            : base(configuration)
        {
            _logger = logger;
            _fileFactory = fileFactory;
            _fileRepository = fileRepository;
            _configuration = configuration;
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
        }

        [HttpGet]
        public IActionResult DownloadById(int id)
        {
            var file = _fileRepository.GetFileById(id);
            if (file is null)
                return NotFound();

            return DownloadFile(file).GetAwaiter().GetResult();
        }

        [HttpGet]
        public IActionResult Download(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var file = _fileRepository.GetFileByDownloadUri(id.Split('/')[^1]);
            if (file is null)
                return NotFound();

            return DownloadFile(file).GetAwaiter().GetResult();
        }

        private async Task<IActionResult> DownloadFile(File file)
        {
            if (!file.Remote)
                return PhysicalFile(file.DiskPath, MediaTypeNames.Application.Octet, file.OriginalName);

            try
            {
                var requestUri = $"{_configuration.GetValue<string>("Remoting:Endpoint")}/stream/{WebUtility.UrlEncode(file.DiskPath)}";
                using var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, HttpContext.RequestAborted);
                response.EnsureSuccessStatusCode();
                var responseStream = await response.Content.ReadAsStreamAsync(HttpContext.RequestAborted);


                var tempPath = System.IO.Path.Combine(_configuration.GetValue<string>("File:StorePath"), System.IO.Path.GetRandomFileName());
                using (System.IO.FileStream targetStream = System.IO.File.Create(tempPath))
                {

                    await responseStream.CopyToAsync(targetStream);
                }
              

                _logger.LogInformation("WE ARE HERE>>>");
                return PhysicalFile(tempPath, MediaTypeNames.Application.Octet, file.OriginalName);

                //Response.Headers.Append("Content-Type", MediaTypeNames.Application.Octet);
                //Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{file.OriginalName}\"");
                //Response.Headers.Append("Transfer-Encoding", "chunked");
                //await responseStream.CopyToAsync(Response.Body, HttpContext.RequestAborted);
                //_logger.LogInformation("COPIED>>>");
                //await Response.CompleteAsync();
                //_logger.LogInformation("DONE>>>");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ISSUE DOWNLKOADING");
            }
            return new EmptyResult();
        }

        [HttpGet]
        public PartialViewResult Details(int id)
        {
            var file = _fileRepository.GetFileById(id);
            if (file is null)
                _logger.LogError("File details request returned null because the file doesn't exist or do not pertain to the user");

            return new PartialViewResult()
            {
                ViewName = "_FileDetails",
                ViewData = GetViewData(file)
            };
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var file = _fileRepository.GetFileById(id);
            if (file is null)
            {
                ModelState.AddModelError("File", "The delete request couldn't be processed");
                _logger.LogError("File delete request returned null because the file doesn't exist or do not pertain to the user");
                return NotFound(ModelState);
            }

            _fileRepository.DeleteFile(file);
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestUtility.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File", "The upload request couldn't be processed");
                _logger.LogError("Request content type is not multi-part");
                return BadRequest(ModelState);
            }

            string boundary = MultipartRequestUtility.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), int.MaxValue);
            MultipartReader reader = new(boundary, HttpContext.Request.Body);
            MultipartSection? section = await reader.ReadNextSectionAsync();

            List<File> uploadedFiles = [];
            while (section is not null)
            {
                if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
                {
                    if (!MultipartRequestUtility.HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File", "The upload request couldn't be processed");
                        _logger.LogError("Request content disposition is missing");
                        return BadRequest(ModelState);
                    }

                    var file = await _fileFactory.CreateFileAsync(contentDisposition, section, ModelState);
                    if (file is not null)
                    {
                        uploadedFiles.Add(file);
                        _fileRepository.SaveFile(file);
                    }
                }

                // Drain any remaining section body that hasn't been consumed and read the headers for the next section
                section = await reader.ReadNextSectionAsync();
            }

            if (uploadedFiles.Count == 0)
                return BadRequest(ModelState);

            return Created(nameof(FileController), uploadedFiles);
        }
    }
}
