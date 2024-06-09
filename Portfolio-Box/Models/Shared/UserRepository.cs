using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models.Shared
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _appDBContext;
        private readonly CookieHandler _cookieHandler;
        private readonly IWebHostEnvironment _environment;

        public UserRepository(AppDBContext appDBContext, CookieHandler cookieHandler, IWebHostEnvironment environment)
        {
            _environment = environment;
            _appDBContext = appDBContext;
            _cookieHandler = cookieHandler;
        }

        public User GetUserByAccessToken()
        {
#if DEBUG
            if (_environment.IsDevelopment())
            {
                return (from user in _appDBContext.Users
                        where user.Id == 1
                        select user)
                        .FirstOrDefault() ?? new AnonymousUser();
            }
#endif

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
