using System.Collections.Generic;

namespace Portfolio_Box.Models.Shared
{
    public interface ISharedLinkRepository
    {
        public IEnumerable<SharedLink> GetSharedLinksByFileId(int id);
    }
}