using System.Collections.Generic;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Models.User
{
	public class AnonymousUser : User
	{
		public override string Nickname { get; set; } = "Anonymous";
		public override string Email { get; set; } = "Anonymous";
		public override int Id { get; set; } = -1;
		public override List<SharedFile> Files { get; set; } = [];
	}
}
