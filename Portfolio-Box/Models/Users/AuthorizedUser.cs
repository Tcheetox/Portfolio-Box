using System.Collections.Generic;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Users
{
    public class AuthorizedUser : User
    {
        public override string? Nickname { get; set; } = "Loading...";
        public override string Email { get; set; } = "Loading...";
        public override int Id { get; set; }
        public override List<File> Files { get; set; } = [];
    }
}
