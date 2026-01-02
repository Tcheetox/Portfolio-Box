namespace Portfolio_Box.Models.Users;

public interface IUserRepository
{
	public User GetUserByAccessToken();
}