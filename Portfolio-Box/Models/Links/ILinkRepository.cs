using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Links;

public interface ILinkRepository
{
	public Link? GetLinkById(int id);
	public void SaveLink(Link sharedLink);
	public File? DeleteLinkById(int id);
}