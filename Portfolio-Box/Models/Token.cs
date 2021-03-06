using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Box.Models
{
    [Table("oauth_tokens")]
    public class Token
    {
        public int Id { get; private set; }

        [Column("user_id")]
        public int UserId { get; private set; }

        [Column("access_token")]
        public string AccessToken { get; private set; }

        [Column("access_token_expires_at")]
        public DateTime AccessTokenExpiresAt { get; private set; }

        public static string ExtractAccessToken(string token)
        {
            if (string.IsNullOrEmpty(token) || !token.Contains('.')) return string.Empty;
            return token.Split('.')[0].Substring(2);
        }
    }
}
