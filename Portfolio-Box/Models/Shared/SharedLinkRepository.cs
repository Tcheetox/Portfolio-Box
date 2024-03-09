using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_Box.Models.Shared
{
	public class SharedLinkRepository(AppDBContext appDBContext) : ISharedLinkRepository
	{
		private readonly AppDBContext _appDBContext = appDBContext;

		public SharedLink? GetLinkById(int id)
			=> (from l in _appDBContext.Links
				where l.Id == id
				select l)
			.Include(f => f.File)
			.FirstOrDefault();


		public void SaveLink(SharedLink sharedLink)
		{
			if (sharedLink.File?.Link is not null)
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

		public SharedFile? DeleteLinkById(int id)
		{
			var targetLink = GetLinkById(id);
			if (targetLink is not null)
			{
				_appDBContext.Links.Remove(targetLink);
				_appDBContext.SaveChanges();
				return targetLink.File;
			}

			return null;
		}
	}
}
