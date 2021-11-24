using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_Box.Models.Shared
{
    public class SharedLinkRepository : ISharedLinkRepository
    {
        private readonly AppDBContext _appDBContext;

        public SharedLinkRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public SharedLink GetLinkById(int id)
        {
            return (from l in _appDBContext.Links
                    where l.Id == id
                    select l)
                    .Include(f => f.File)
                    .FirstOrDefault();
        }

        public void SaveLink(SharedLink sharedLink)
        {
            if (sharedLink.File.Link != null)
            {
                sharedLink.UpdateFrom(sharedLink.File.Link);
                _appDBContext.Links.Update(sharedLink);
            }
            else
            {
                _appDBContext.Links.Add(sharedLink);
            }
            _appDBContext.SaveChanges();
        }

        public void DeleteLinkById(int id)
        {
            SharedLink targetLink = GetLinkById(id);
            if (targetLink != null)
            {
                _appDBContext.Links.Remove(targetLink);
                _appDBContext.SaveChanges();
            }
        }
    }
}
