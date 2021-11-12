using Portfolio_Box.Models.Files;
using System.Collections.Generic;

namespace Portfolio_Box.Models.Shared
{
    public class SharedLinkRepository : ISharedLinkRepository
    {
        public IEnumerable<SharedLink> AllLinks { get; }
    }
}
