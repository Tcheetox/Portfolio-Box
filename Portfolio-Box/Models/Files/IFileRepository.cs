using System.Collections.Generic;

namespace Portfolio_Box.Models.Files
{
    public interface IFileRepository
    {
        public IEnumerable<File> AllFiles { get; }
        public File? GetFileById(int id);
        public File? GetFileByDownloadUri(string downloadUri);
        public void SaveFile(File sharedFile);
        public void DeleteFile(File sharedFile);
    }
}