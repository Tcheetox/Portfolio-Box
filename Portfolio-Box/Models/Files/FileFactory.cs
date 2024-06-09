using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models.Files
{
    public class FileFactory(ILogger<FileFactory> logger, User user, IConfiguration configuration) : IFileFactory
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<FileFactory> _logger = logger;
        private readonly User _user = user;

        public void DeleteFile(File sharedFile)
        {
            Task.Run(() =>
            {
                try
                {
                    System.IO.File.Delete(sharedFile.DiskPath);
                    _logger.LogInformation("File '{OriginalName}' deleted from disk by user '{Nickname}' ({Id})",
                        sharedFile.OriginalName, _user.Nickname, _user.Id);
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "Unexpected error while deleting file '{OriginalName}' from disk '{DiskPath}'",
                            sharedFile.OriginalName, sharedFile.DiskPath);
                }
            });
        }

        public async Task<File?> TryCreateFile(ContentDispositionHeaderValue contentDisposition, MultipartSection section, ModelStateDictionary modelState)
        {
            File? sharedFile = null;
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value) ?? string.Empty;
            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            var targetFilePath = _configuration.GetValue<string>("File:StorePath") ?? string.Empty;

            using (FileStream targetStream = System.IO.File.Create(Path.Combine(targetFilePath, trustedFileNameForFileStorage)))
            {
                try
                {
                    await section.Body.CopyToAsync(targetStream);
                    sharedFile = new File(_user.Id, Path.Combine(targetFilePath, trustedFileNameForFileStorage), trustedFileNameForDisplay, targetStream.Length);
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
