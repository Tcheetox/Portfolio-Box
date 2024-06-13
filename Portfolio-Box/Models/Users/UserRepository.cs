using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Portfolio_Box.Models.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly AppDBContext _appDBContext;
        private readonly CookieHandler _cookieHandler;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public UserRepository(
            ILogger<UserRepository> logger,
            AppDBContext appDBContext, 
            CookieHandler cookieHandler, 
            IWebHostEnvironment environment, 
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _appDBContext = appDBContext;
            _cookieHandler = cookieHandler;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        private bool TryGetUserFromIP([NotNullWhen(true)] out User? user)
        {
            user = null;
            if (_contextAccessor.HttpContext is null)
                return false;

            try
            {
                var adminHost = _configuration.GetValue<string>("Remoting:Host")!;
                var ips = Dns.GetHostAddresses(adminHost);
                var callerIp = _contextAccessor.HttpContext.Connection.RemoteIpAddress;

                _logger.LogWarning("adminHost: {adminHost}", adminHost);
                _logger.LogWarning("ips: {ips}", string.Join('-', ips.Select(e => e.ToString())));
                _logger.LogWarning("callerIp: {callerIp}", callerIp.ToString());

                if (ips is null || callerIp is null || !ips.Contains(callerIp))
                    return false;

                user = _appDBContext.Users.OfType<AdminUser>().FirstOrDefault();
                return user is not null;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error identifying user from it's IP");
            }

            return false;
        }

        public User GetUserByAccessToken()
        {
            if (TryGetUserFromIP(out var userByIp))
                return userByIp;

#if DEBUG
            if (_environment.IsDevelopment())
            {
                return (from user in _appDBContext.Users
                        where user.Id == 1
                        select user)
                        .FirstOrDefault() ?? AnonymousUser.Instance;
            }
#endif

            if (!_cookieHandler.TryGetCookie(out var cookie))
                return AnonymousUser.Instance;

            string accessToken = Token.ExtractAccessToken(cookie.Value);
            var u = (from token in _appDBContext.Tokens
                     join user in _appDBContext.Users on token.UserId equals user.Id
                     where token.AccessToken == accessToken && token.AccessTokenExpiresAt > DateTime.Now
                     select user)
                     .FirstOrDefault();

            return u ?? AnonymousUser.Instance;
        }
    }
}
