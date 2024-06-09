using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_Box.Models.Shared
{
    public class SharedFileRepository(AppDBContext dbContext, User.User user, ISharedFileFactory fileFactory) : ISharedFileRepository
    {
        private readonly AppDBContext _appDBContext = dbContext;
        private readonly User.User _user = user;
        private readonly ISharedFileFactory _sharedFileFactory = fileFactory;

        public IEnumerable<SharedFile> AllFiles
            => from f in _appDBContext.Files
               where f.UserId == _user.Id
               orderby f.UploadedOn descending
               select f;

        public SharedFile? GetFileById(int id)
            => (from f in _appDBContext.Files
                where f.Id == id && f.UserId == _user.Id
                select f)
            .Include(c => c.Link)
            .FirstOrDefault();

        public SharedFile? GetFileByDownloadUri(string downloadUri)
            => (from f in _appDBContext.Files
                where f.Link != null && f.Link.DownloadUri == downloadUri && f.Link.Expiration > DateTime.Now
                select f)
            .FirstOrDefault();

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
