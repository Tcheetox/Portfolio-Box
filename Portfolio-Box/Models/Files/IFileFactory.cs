using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Portfolio_Box.Models.Files
{
    public interface IFileFactory
    {
        public Task<File?> CreateFileAsync(ContentDispositionHeaderValue contentDisposition, MultipartSection section, ModelStateDictionary modelState, CancellationToken token = default);
        public Task<File[]> CreateFilesAsync(IEnumerable<RemoteFileDto> remoteFiles, CancellationToken token = default);
        public Task DeleteFileAsync(File sharedFile, CancellationToken token = default);
    }
}