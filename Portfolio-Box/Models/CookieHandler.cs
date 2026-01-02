using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Portfolio_Box.Models;

[NotMapped]
public class CookieHandler
{
	private readonly IConfiguration _configuration;
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly ILogger<CookieHandler> _logger;

	public CookieHandler(IHttpContextAccessor contextAccessor, IConfiguration configuration, ILogger<CookieHandler> logger)
	{
		_contextAccessor = contextAccessor;
		_configuration = configuration;
		_logger = logger;
	}

	public bool TryGetCookie(out KeyValuePair<string, string> cookie)
	{
		var cookieName = _configuration.GetValue<string>("Cookies:Access") ?? string.Empty;
		cookie = new KeyValuePair<string, string>(cookieName, string.Empty);

		var target = _contextAccessor
			?.HttpContext
			?.Request
			?.Cookies
			?.FirstOrDefault(c => c.Key == cookieName);

		if (target?.Value is null)
			return false;

		cookie = new KeyValuePair<string, string>(cookieName, target.Value.Value);
		return true;
	}

	public void KillAll(HttpResponse httpResponse)
	{
		var cookiePath = _configuration.GetValue<string>("Cookies:Path");
		if (cookiePath is null)
			return;

		var domain = _configuration.GetValue<string>("Cookies:Domain");
		foreach (var cookie in _configuration
			         .GetSection("Cookies")
			         .GetChildren()
			         .Select(c => c.Value))
		{
			try
			{
				httpResponse.Cookies.Delete(
					cookie!,
					new CookieOptions { Domain = domain, Path = cookiePath });
			}
			catch (Exception e)
			{
				// Swallow.
				_logger.LogError(e, "Error while deleting cookie '{CookieName}'", cookie);
			}
		}
	}
}