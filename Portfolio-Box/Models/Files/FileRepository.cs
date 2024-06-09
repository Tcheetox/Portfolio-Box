using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models.Files
{
    public class FileRepository(AppDBContext dbContext, User user, IFileFactory fileFactory) : IFileRepository
    {
        private readonly AppDBContext _appDBContext = dbContext;
        private readonly User _user = user;
        private readonly IFileFactory _sharedFileFactory = fileFactory;

        public IEnumerable<File> AllFiles
            => from f in _appDBContext.Files
               where f.UserId == _user.Id
               orderby f.UploadedOn descending
               select f;

        public File? GetFileById(int id)
            => (from f in _appDBContext.Files
                where f.Id == id && f.UserId == _user.Id
                select f)
            .Include(c => c.Link)
            .FirstOrDefault();

        public File? GetFileByDownloadUri(string downloadUri)
            => (from f in _appDBContext.Files
                where f.Link != null && f.Link.DownloadUri == downloadUri && f.Link.Expiration > DateTime.Now
                select f)
            .FirstOrDefault();

        public void SaveFile(File sharedFile)
        {
            _appDBContext.Files.Add(sharedFile);
            _appDBContext.SaveChanges();
        }

        public void DeleteFile(File sharedFile)
        {
            _appDBContext.Files.Remove(sharedFile);
            _appDBContext.SaveChanges();
            _sharedFileFactory.DeleteFile(sharedFile);
        }
    }
}
