using System.Collections.Generic;

namespace Portfolio_Box.Models.Files
{
    public interface ISharedFileRepository
    {
        public IEnumerable<SharedFile> AllFiles { get; }

        public SharedFile GetSharedFileById(int id);
    }
}