using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Portfolio_Box.Attributes;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;
using Portfolio_Box.Utilities;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

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
            _sharedFileRepository = sharedFileRepository;
            _sharedFileFactory = sharedFileFactory;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("SharedFile", "The request couldn't be processed (Error 1)");
                _logger.LogError("Request content type is not multi-part");
                return BadRequest(ModelState);
            }

            string boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), int.MaxValue);
            MultipartReader reader = new MultipartReader(boundary, HttpContext.Request.Body);
            MultipartSection section = await reader.ReadNextSectionAsync();

            (bool status, SharedFile? file) uploadRequest = (false, null);
            while (section != null)
            {
                if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File", "The request couldn't be processed (Error 2)");
                        _logger.LogError("Request content disposition is missing");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        uploadRequest = await _sharedFileFactory.TryCreateFile(contentDisposition, section, ModelState);
                    }

                // Drain any remaining section body that hasn't been consumed and read the headers for the next section
                section = await reader.ReadNextSectionAsync();
            }

            if (uploadRequest.status && uploadRequest.file != null)
            {
                _sharedFileRepository.SaveFile(uploadRequest.file);
                return Created(nameof(FileController), uploadRequest.file);
            }

            return BadRequest(ModelState);
        }
    }
}
