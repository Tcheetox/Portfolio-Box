using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_Box.Models.Shared
{
    public class SharedFileRepository : ISharedFileRepository
    { 
        private readonly AppDBContext _appDBContext;
        private readonly User.User _user;
        private readonly ISharedFileFactory _sharedFileFactory;

        public SharedFileRepository(AppDBContext appDBContext, User.User user, ISharedFileFactory sharedFileFactory)
        {
            _appDBContext = appDBContext;
            _user = user;
            _sharedFileFactory = sharedFileFactory;
        }

        public IEnumerable<SharedFile> AllFiles => from f in _appDBContext.Files
                                                   where f.UserId == _user.Id
                                                   select f;

        public SharedFile GetFileById(int id)
        {
            return (from f in _appDBContext.Files
                    where f.Id == id && f.UserId == _user.Id
                    select f)
                    .Include(c => c.Link)
                    .FirstOrDefault();
        }

        public SharedFile GetFileByDownloadUri(string downloadUri)
        {
            return (from f in _appDBContext.Files
                    where f.Link != null && f.Link.DownloadUri == downloadUri
                    select f)
                    .FirstOrDefault();
        }

        public void SaveFile(SharedFile sharedFile)
        {
            _appDBContext.Files.Add(sharedFile);
            _appDBContext.SaveChanges();
        }

        public void DeleteFileById(int id)
        {
            SharedFile file = GetFileById(id);
            if (file != null)
            {
                _appDBContext.Files.Remove(file);
                _appDBContext.SaveChanges();
                _sharedFileFactory.DeleteFile(file);
            }
        }
    }
}
