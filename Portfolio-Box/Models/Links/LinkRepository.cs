using System.Linq;
using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Links;

public class LinkRepository : ILinkRepository
{
	private readonly AppDbContext _appDbContext;

	public LinkRepository(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}

	public Link? GetLinkById(int id)
	{
		return (from l in _appDbContext.Links
				where l.Id == id
				select l)
			.Include(f => f.File)
			.FirstOrDefault();
	}


	public void SaveLink(Link sharedLink)
	{
		if (sharedLink.File?.Link is not null)
		{
			sharedLink.UpdateFrom(sharedLink.File.Link);
			_appDbContext.Links.Update(sharedLink);
		}
		else
		{
			_appDbContext.Links.Add(sharedLink);
		}

		_appDbContext.SaveChanges();
	}

	public File? DeleteLinkById(int id)
	{
		var targetLink = GetLinkById(id);
		if (targetLink is not null)
		{
			_appDbContext.Links.Remove(targetLink);
			_appDbContext.SaveChanges();
			return targetLink.File;
		}

		return null;
	}
}