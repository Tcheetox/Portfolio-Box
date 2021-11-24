
namespace Portfolio_Box.Models.Shared
{
    public interface ISharedLinkRepository
    {
        public SharedLink GetLinkById(int id);
        public void SaveLink(SharedLink sharedLink);
        public void DeleteLinkById(int id);
    }
}