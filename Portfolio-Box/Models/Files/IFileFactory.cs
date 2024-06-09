using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Portfolio_Box.Models.Files
{
    public interface IFileFactory
    {
        public Task<File?> TryCreateFile(ContentDispositionHeaderValue contentDisposition, MultipartSection section, ModelStateDictionary modelState);

        public void DeleteFile(File sharedFile);
    }
}