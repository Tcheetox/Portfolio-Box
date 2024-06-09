using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;
using Portfolio_Box.Pages;
using Portfolio_Box.Utilities;

namespace Portfolio_Box.Controllers
{
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly ISharedFileRepository _sharedFileRepository;
        private readonly ISharedFileFactory _sharedFileFactory;
        private readonly User _user;

        public FileController(ILogger<FileController> logger, User user, ISharedFileRepository sharedFileRepository, ISharedFileFactory sharedFileFactory)
        {
            _logger = logger;
            _user = user;
            _sharedFileFactory = sharedFileFactory;
            _sharedFileRepository = sharedFileRepository;
        }

        [HttpGet]
        public IActionResult DownloadById(int id)
        {
            var file = _sharedFileRepository.GetFileById(id);
            if (file is null)
                return NotFound();

            return PhysicalFile(file.DiskPath, MediaTypeNames.Application.Octet, file.OriginalName);
        }

        [HttpGet]
        public IActionResult Download(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("FileNotFound", new FileNotFoundModel(_user));

            var file = _sharedFileRepository.GetFileByDownloadUri(id.Split('/')[^1]);
            if (file is null)
                return View("FileNotFound", new FileNotFoundModel(_user));

            return PhysicalFile(file.DiskPath, MediaTypeNames.Application.Octet, file.OriginalName);
        }

        [HttpGet]
        public PartialViewResult Details(int id)
        {
            var file = _sharedFileRepository.GetFileById(id);
            if (file is null)
                _logger.LogError("File details request returned null because the file doesn't exist or do not pertain to the user");

            return new PartialViewResult()
            {
                ViewName = "_FileDetails",
                ViewData = new ViewDataDictionary<SharedFile>(ViewData, file)
            };
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var file = _sharedFileRepository.GetFileById(id);
            if (file is null)
            {
                ModelState.AddModelError("File", "The delete request couldn't be processed");
                _logger.LogError("File delete request returned null because the file doesn't exist or do not pertain to the user");
                return NotFound(ModelState);
            }

            _sharedFileRepository.DeleteFile(file);
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File", "The upload request couldn't be processed");
                _logger.LogError("Request content type is not multi-part");
                return BadRequest(ModelState);
            }

            string boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), int.MaxValue);
            MultipartReader reader = new(boundary, HttpContext.Request.Body);
            MultipartSection? section = await reader.ReadNextSectionAsync();

            List<SharedFile> uploadedFiles = [];
            while (section is not null)
            {
                if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File", "The upload request couldn't be processed");
                        _logger.LogError("Request content disposition is missing");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        SharedFile? file = await _sharedFileFactory.TryCreateFile(contentDisposition, section, ModelState);
                        if (file != null)
                        {
                            uploadedFiles.Add(file);
                            _sharedFileRepository.SaveFile(file);
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
