using System.Collections.Generic;

namespace Portfolio_Box.Models.Shared
{
    public interface ISharedFileRepository
    {
        public IEnumerable<SharedFile> AllFiles { get; }
        public SharedFile GetFileById(int id);
        public SharedFile GetFileByDownloadUri(string downloadUri);
        public void SaveFile(SharedFile sharedFile);
        public void DeleteFile(SharedFile sharedFile);
    }
}