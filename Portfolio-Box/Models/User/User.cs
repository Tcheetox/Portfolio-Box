using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.DependencyInjection;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Models.User
{
    [NotMapped]
    public abstract class User
    {
        public abstract string Nickname { get; set; }
        public abstract string Email { get; set; }
        public abstract int Id { get; set; }
        public abstract List<SharedFile> Files { get; set; }

        public abstract void Logout();

        public static User GetUser(IServiceProvider serviceProvider)
        {
            if (!serviceProvider.GetRequiredService<Cookie>().TryGetCookie(out var cookie))
                return new AnonymousUser();

            var ctx = serviceProvider.GetService<AppDBContext>();
            var accessToken = Token.ExtractAccessToken(cookie.Value);
            var token = Token.FindByAccessToken(ctx, accessToken);
            if (token == null) return new AnonymousUser();

            var user = ctx.Users.Find(token.UserId);
            if (user != null) return user;
            else return new AnonymousUser();
        }
    }
}
