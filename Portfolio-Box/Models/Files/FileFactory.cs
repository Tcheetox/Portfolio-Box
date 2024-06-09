using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models.Files
{
    public class FileFactory : IFileFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileFactory> _logger;
        private readonly User _user;

        public FileFactory(ILogger<FileFactory> logger, User user, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _user = user;
        }

        public Task DeleteFileAsync(File sharedFile, CancellationToken token = default)
            => Task.Run(() =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    System.IO.File.Delete(sharedFile.DiskPath);
                    _logger.LogInformation("File '{OriginalName}' deleted from disk by user '{Nickname}' ({Id})",
                        sharedFile.OriginalName, _user.Nickname, _user.Id);
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "Unexpected error while deleting file '{OriginalName}' from disk '{DiskPath}'",
                            sharedFile.OriginalName, sharedFile.DiskPath);
                }
            }, token);

        public async Task<File?> CreateFileAsync(ContentDispositionHeaderValue contentDisposition, MultipartSection section, ModelStateDictionary modelState, CancellationToken token = default)
        {
            File? sharedFile = null;
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value) ?? string.Empty;
            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            var targetFilePath = Path.Combine(_configuration.GetValue<string>("File:StorePath") ?? string.Empty, trustedFileNameForFileStorage);

            using (FileStream targetStream = System.IO.File.Create(targetFilePath))
            {
                try
                {
                    await section.Body.CopyToAsync(targetStream, token);
                    sharedFile = new File(_user.Id, targetFilePath, trustedFileNameForDisplay, targetStream.Length);
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "Unexpected error while writing file '{TrustedFileNameForDisplay}' to disk '{TargetFilePath}' as '{TrustedFileNameForFileStorage}'",
                        trustedFileNameForDisplay, targetFilePath, trustedFileNameForFileStorage);
                    modelState.AddModelError("File", "The file couldn't be saved (Error 3)");
                }
            }

            return sharedFile;
        }
    }
}
