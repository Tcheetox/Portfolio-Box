using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Users;

[Table("users")]
public abstract class User
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public abstract int Id { get; set; }

	public abstract string? Nickname { get; set; }

	[Required]
	public abstract string Email { get; set; }

	public abstract List<File> Files { get; set; }
}