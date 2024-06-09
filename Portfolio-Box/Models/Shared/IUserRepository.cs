using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models.Shared
{
    public interface IUserRepository
    {
        public User GetUserByAccessToken();
    }
}
