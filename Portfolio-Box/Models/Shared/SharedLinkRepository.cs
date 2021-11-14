using System.Collections.Generic;
using System.Linq;

namespace Portfolio_Box.Models.Shared
{
    public class SharedLinkRepository : ISharedLinkRepository
    {
        private readonly AppDBContext _appDBContext;
        public SharedLinkRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public IEnumerable<SharedLink> GetSharedLinksByFileId(int id)
        {
            return from l in _appDBContext.Links
                   where l.SharedFileId == id
                   select l;
        }
    }
}
