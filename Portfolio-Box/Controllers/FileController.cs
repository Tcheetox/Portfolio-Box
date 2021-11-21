using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Portfolio_Box.Attributes;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;
using Portfolio_Box.Utilities;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Portfolio_Box.Controllers
{
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly ISharedFileRepository _sharedFileRepository;
        private readonly User _user;

        public FileController(ILogger<FileController> logger, User user, ISharedFileRepository sharedFileRepository)
        {
            _logger = logger;
            _user = user;
            _sharedFileRepository = sharedFileRepository;
        }

        [HttpPost]
        //[DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("SharedFile", "The request couldn't be processed (Error 1)");
                // TODO: Log error
                return BadRequest(ModelState);
            }

            // TODO: check
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 999999999);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File", "The request couldn't be processed (Error 2)");
                        // TODO: Log error
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        // Don't trust the file name sent by the client. To display the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();
                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(section, contentDisposition, ModelState, null, 10737418240);

                        if (!ModelState.IsValid)
                            return BadRequest(ModelState);

                        string targetFilePath = @"C:\Users\kevin\Desktop\STORE";

                        using (var targetStream = System.IO.File.Create(Path.Combine(targetFilePath, trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);

                            _logger.LogInformation(
                                "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
                                "'{TargetFilePath}' as {TrustedFileNameForFileStorage}",
                                trustedFileNameForDisplay, targetFilePath,
                                trustedFileNameForFileStorage);
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return Created(nameof(FileController), null);
        }

    }
}
