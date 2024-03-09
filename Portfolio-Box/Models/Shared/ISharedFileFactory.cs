using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Portfolio_Box.Models.Shared
{
	public interface ISharedFileFactory
	{
		public Task<SharedFile?> TryCreateFile(ContentDispositionHeaderValue contentDisposition, MultipartSection section, ModelStateDictionary modelState);

		public void DeleteFile(SharedFile sharedFile);
	}
}