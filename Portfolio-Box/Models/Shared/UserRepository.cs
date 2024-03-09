using System;
using System.Linq;
using Portfolio_Box.Models.User;

namespace Portfolio_Box.Models.Shared
{
	public class UserRepository(AppDBContext appDBContext, CookieHandler cookieHandler) : IUserRepository
	{
		private readonly AppDBContext _appDBContext = appDBContext;
		private readonly CookieHandler _cookieHandler = cookieHandler;

		public User.User GetUserByAccessToken()
		{
			if (!_cookieHandler.TryGetCookie(out var cookie))
				return new AnonymousUser();

			string accessToken = Token.ExtractAccessToken(cookie.Value);

			var u = (from token in _appDBContext.Tokens
					 join user in _appDBContext.Users on token.UserId equals user.Id
					 where token.AccessToken == accessToken && token.AccessTokenExpiresAt > DateTime.Now
					 select user)
					 .FirstOrDefault();

			return u ?? new AnonymousUser();
		}
	}
}
