using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Portfolio_Box.Models.Users;

public class UserRepository : IUserRepository
{
	private readonly AppDbContext _appDbContext;
	private readonly IConfiguration _configuration;
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly CookieHandler _cookieHandler;
	private readonly IWebHostEnvironment _environment;
	private readonly ILogger<UserRepository> _logger;

	public UserRepository(
		ILogger<UserRepository> logger,
		AppDbContext appDbContext,
		CookieHandler cookieHandler,
		IWebHostEnvironment environment,
		IHttpContextAccessor contextAccessor,
		IConfiguration configuration)
	{
		_logger = logger;
		_environment = environment;
		_appDbContext = appDbContext;
		_cookieHandler = cookieHandler;
		_contextAccessor = contextAccessor;
		_configuration = configuration;
	}

	public User GetUserByAccessToken()
	{
		if (TryGetUserFromIp(out var userByIp))
			return userByIp;

#if DEBUG
		if (_environment.IsDevelopment())
		{
			return (from user in _appDbContext.Users
					where user.Id == 1
					select user)
				.FirstOrDefault() ?? AnonymousUser.Instance;
		}
#endif

		if (!_cookieHandler.TryGetCookie(out var cookie))
			return AnonymousUser.Instance;

		var accessToken = Token.ExtractAccessToken(cookie.Value);
		var u = (from token in _appDbContext.Tokens
				join user in _appDbContext.Users on token.UserId equals user.Id
				where token.AccessToken == accessToken && token.AccessTokenExpiresAt > DateTime.Now
				select user)
			.FirstOrDefault();

		return u ?? AnonymousUser.Instance;
	}

	private bool TryGetUserFromIp([NotNullWhen(true)] out User? user)
	{
		user = null;
		if (_contextAccessor.HttpContext is null)
			return false;

		try
		{
			var ips = Dns.GetHostAddresses(_configuration.GetValue<string>("Remoting:Host")!).Select(i => i.ToString()).ToHashSet();
			var callerIp = IPAddress.Parse(_contextAccessor.HttpContext.Request.Headers["X-Real-IP"]!).ToString();
			if (!ips.Contains(callerIp))
				return false;

			user = _appDbContext.Users.OfType<AdminUser>().FirstOrDefault();
			return user is not null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error identifying user from it's IP");
		}

		return false;
	}
}