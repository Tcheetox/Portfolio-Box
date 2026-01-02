using System.Linq;
using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Links;

public class LinkRepository : ILinkRepository
{
	private readonly AppDBContext _appDBContext;

	public LinkRepository(AppDBContext appDBContext)
	{
		_appDBContext = appDBContext;
	}

	public Link? GetLinkById(int id)
	{
		return (from l in _appDBContext.Links
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
			_appDBContext.Links.Update(sharedLink);
		}
		else
		{
			_appDBContext.Links.Add(sharedLink);
		}

		_appDBContext.SaveChanges();
	}

	public File? DeleteLinkById(int id)
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