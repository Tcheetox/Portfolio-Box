using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Portfolio_Box.Models.Shared
{
    public class SharedFileFactory : ISharedFileFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SharedFileFactory> _logger;
        private readonly User.User _user;
        public SharedFileFactory(ILogger<SharedFileFactory> logger, User.User user, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _user = user;
        }

        public void DeleteFile(SharedFile sharedFile)
        {
            Task.Run(() =>
            {
                try
                {
                    File.Delete(sharedFile.DiskPath);
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

        public async Task<(bool, SharedFile?)> TryCreateFile(ContentDispositionHeaderValue contentDisposition, MultipartSection section, ModelStateDictionary modelState)
        {
            (bool status, SharedFile? file) output = (false, null);
            string trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
            string trustedFileNameForFileStorage = Path.GetRandomFileName();
            string targetFilePath = _configuration.GetValue<string>("File:StorePath");
            // _configuration.GetValue<long>("File:MaxBytes")

            // TODO: handle max size
            using (FileStream targetStream = File.Create(Path.Combine(targetFilePath, trustedFileNameForFileStorage)))
            {
                try
                {
                    await section.Body.CopyToAsync(targetStream);
                    SharedFile sharedFile = new SharedFile(_user.Id, Path.Combine(targetFilePath, trustedFileNameForFileStorage), trustedFileNameForDisplay, targetStream.Length);
                    output = (true, sharedFile);
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "Unexpected error while writing file '{TrustedFileNameForDisplay}' to disk '{TargetFilePath}' as '{TrustedFileNameForFileStorage}'",
                        trustedFileNameForDisplay, targetFilePath, trustedFileNameForFileStorage);
                    modelState.AddModelError("File", "The file couldn't be saved (Error 3)");
                }
            }

            return output;
        }
    }
}
