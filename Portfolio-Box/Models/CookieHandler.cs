using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Portfolio_Box.Models
{
    [NotMapped]
    public class CookieHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public CookieHandler(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public bool TryGetCookie(out KeyValuePair<string,string> cookie)
        {
            string cookieName = _configuration.GetValue<string>("Cookies:Access");

            cookie = new KeyValuePair<string, string>(cookieName, string.Empty);
            var target = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request
                .Cookies?.FirstOrDefault(c => c.Key == cookieName);

            if (target == null || target.Value.Value == null) return false;

            cookie = new KeyValuePair<string,string>(cookieName, target.Value.Value.ToString());
            return true;
        }

        public void KillAll(HttpResponse httpResponse)
        {
            string cookiePath = _configuration.GetValue<string>("Cookies:Path");
            foreach (var cookie in _configuration.GetSection("Cookies").GetChildren())
            {
                Debug.WriteLine(cookie.Path);
                httpResponse.Cookies.Delete(cookie.Value, new CookieOptions() { Expires = DateTime.Now.AddDays(-1), Path = cookiePath });
            }
        }
    }
}
