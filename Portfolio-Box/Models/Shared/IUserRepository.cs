namespace Portfolio_Box.Models.Shared
{
    public interface IUserRepository
    {
        public User.User GetUserByAccessToken();
    }
}
