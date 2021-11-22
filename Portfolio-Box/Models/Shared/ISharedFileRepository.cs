using System.Collections.Generic;

namespace Portfolio_Box.Models.Shared
{
    public interface ISharedFileRepository
    {
        public IEnumerable<SharedFile> AllFiles { get; }

        public SharedFile GetSharedFileById(int id);

        public void SaveFile(SharedFile sharedFile);
    }
}