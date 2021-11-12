using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_Box.Models.Files
{
    public class SharedFileRepository : ISharedFileRepository
    { 
        private readonly AppDBContext _appDBContext;
        public SharedFileRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public IEnumerable<SharedFile> AllFiles => _appDBContext.Files;

        public SharedFile GetSharedFileById(int id)
        {
            return _appDBContext.Files.Include(c => c.Links).FirstOrDefault(f => f.Id == id);
        }
    }
}
