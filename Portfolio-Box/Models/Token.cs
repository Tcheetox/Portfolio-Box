using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Box.Models
{
	[Table("oauth_tokens")]
	public class Token
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [ForeignKey("User")]
        [Column("user_id")]
		public int UserId { get; private set; }

		[Column("access_token")]
		public string? AccessToken { get; private set; }

		[Column("access_token_expires_at")]
		public DateTime AccessTokenExpiresAt { get; private set; }

		public static string ExtractAccessToken(string token)
			=> string.IsNullOrEmpty(token) || !token.Contains('.') ? string.Empty : token.Split('.')[0][2..];
	}
}
