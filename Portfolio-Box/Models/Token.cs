using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;

namespace Portfolio_Box.Models
{
    [Table("oauth_tokens")]
    public class Token
    {
        public int Id { get; private set; }

        [Column("user_id")]
        public uint UserId { get; private set; }

        [Column("access_token")]
        public string AccessToken { get; private set; }

        public static Token FindByAccessToken(AppDBContext ctx, string token)
        {
            return ctx.Tokens.FirstOrDefault(t => t.AccessToken == token);
        }

        public static string ExtractAccessToken(string token)
        {
            if (string.IsNullOrEmpty(token) || !token.Contains('.')) return string.Empty;
            return token.Split('.')[0].Substring(2);
        }
    }
}
