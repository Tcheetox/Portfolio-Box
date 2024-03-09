using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Models.User
{
	[Table("users")]
	public abstract class User
	{
		public abstract string? Nickname { get; set; }
		public abstract string Email { get; set; }
		public abstract int Id { get; set; }
		public abstract List<SharedFile> Files { get; set; }
	}
}
