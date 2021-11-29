using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

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
                                                   orderby f.UploadedOn descending
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
                    where f.Link != null && f.Link.DownloadUri == downloadUri && f.Link.Expiration > DateTime.Now
                    select f)
                    .FirstOrDefault();
        }

        public void SaveFile(SharedFile sharedFile)
        {
            _appDBContext.Files.Add(sharedFile);
            _appDBContext.SaveChanges();
        }

        public void DeleteFile(SharedFile sharedFile)
        {
            _appDBContext.Files.Remove(sharedFile);
            _appDBContext.SaveChanges();
            _sharedFileFactory.DeleteFile(sharedFile);
        }
    }
}
