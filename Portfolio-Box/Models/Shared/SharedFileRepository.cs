using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_Box.Models.Shared
{
    public class SharedFileRepository : ISharedFileRepository
    { 
        private readonly AppDBContext _appDBContext;
        private readonly User.User _user;
        public SharedFileRepository(AppDBContext appDBContext, User.User user)
        {
            _appDBContext = appDBContext;
            _user = user;
        }

        public IEnumerable<SharedFile> AllFiles => _appDBContext.Files.Where(f => f.UserId == _user.Id);

        public SharedFile GetSharedFileById(int id)
        {
            return _appDBContext.Files.Include(c => c.Links).FirstOrDefault(f => f.Id == id && f.UserId == _user.Id);
        }
    }
}
