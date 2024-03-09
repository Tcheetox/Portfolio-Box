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
	public class SharedFileFactory(ILogger<SharedFileFactory> logger, User.User user, IConfiguration configuration) : ISharedFileFactory
	{
		private readonly IConfiguration _configuration = configuration;
		private readonly ILogger<SharedFileFactory> _logger = logger;
		private readonly User.User _user = user;

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

		public async Task<SharedFile?> TryCreateFile(ContentDispositionHeaderValue contentDisposition, MultipartSection section, ModelStateDictionary modelState)
		{
			SharedFile? sharedFile = null;
			var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value) ?? string.Empty;
			var trustedFileNameForFileStorage = Path.GetRandomFileName();
			var targetFilePath = _configuration.GetValue<string>("File:StorePath") ?? string.Empty;

			using (FileStream targetStream = File.Create(Path.Combine(targetFilePath, trustedFileNameForFileStorage)))
			{
				try
				{
					await section.Body.CopyToAsync(targetStream);
					sharedFile = new SharedFile(_user.Id, Path.Combine(targetFilePath, trustedFileNameForFileStorage), trustedFileNameForDisplay, targetStream.Length);
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
