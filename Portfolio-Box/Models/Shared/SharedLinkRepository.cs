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

        // TODO: check!
        public IEnumerable<SharedLink> AllLinks => _appDBContext.Links;
    }
}
