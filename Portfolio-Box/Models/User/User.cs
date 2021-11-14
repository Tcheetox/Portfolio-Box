using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Models.User
{
    [Table("users")]
    public abstract class User
    {
        public abstract string Nickname { get; set; }
        public abstract string Email { get; set; }
        public abstract int Id { get; set; }
        public abstract List<SharedFile> Files { get; set; }

        public static User GetUser(IServiceProvider serviceProvider)
        {
            if (!serviceProvider.GetRequiredService<CookieHandler>().TryGetCookie(out var cookie))
                return new AnonymousUser();

            var ctx = serviceProvider.GetService<AppDBContext>();
            var token = Token.FindByAccessToken(ctx, Token.ExtractAccessToken(cookie.Value));
            if (token == null) return new AnonymousUser();

            var user = ctx.Users.Find(token.UserId);
            if (user != null) return user;
            else return new AnonymousUser();
        }
    }
}
