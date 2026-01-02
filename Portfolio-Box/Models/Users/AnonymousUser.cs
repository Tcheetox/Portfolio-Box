using System;
using System.Collections.Generic;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Users;

public class AnonymousUser : User
{
	private static readonly Lazy<AnonymousUser> _instance = new(() => new AnonymousUser());
	public static AnonymousUser Instance => _instance.Value;

	public override string? Nickname { get; set; } = "Anonymous";
	public override string Email { get; set; } = "Anonymous";
	public override int Id { get; set; } = -1;
	public override List<File> Files { get; set; } = [];
}