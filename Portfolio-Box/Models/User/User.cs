using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Portfolio_Box.Models.User
{
    public abstract class User
    {
        public abstract string Nickname { get; set; }

        public abstract string Email { get; set; }

        public abstract int Id { get; set; }

        public abstract void Logout();

        public static User GetUser(IServiceProvider serviceProvider)
        {
            var cookie = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request
                .Cookies?.FirstOrDefault(c => c.Key == "accessTokenKecha");
            if (!cookie.HasValue || cookie.Value.Value == null) return new AnonymousUser();

            var ctx = serviceProvider.GetService<AppDBContext>();
            var token = Token.FindByAccessToken(ctx, cookie.Value.Value.ToString());
            if (token == null) return new AnonymousUser();

            return ctx.Users.Find(token.UserId) ?? new AnonymousUser();
        }
    }
}
