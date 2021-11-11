using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Portfolio_Box.Models
{
    public class Cookie
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public Cookie(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public bool TryGetCookie(out KeyValuePair<string,string> cookie)
        {
            string cookieName = _configuration.GetValue<string>("Hosting:CookieName");

            cookie = new KeyValuePair<string, string>(cookieName, string.Empty);
            var target = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request
                .Cookies?.FirstOrDefault(c => c.Key == cookieName);

            if (target == null || target.Value.Value == null) return false;

            cookie = new KeyValuePair<string,string>(cookieName, target.Value.Value.ToString());
            return true;
        }
    }
}
