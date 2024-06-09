using System.Collections.Generic;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Users
{
    public class AuthorizedUser : User
    {
        public override string? Nickname { get; set; }
        public override required string Email { get; set; }
        public override int Id { get; set; }
        public override int GroupId { get; set; }
        public override List<File> Files { get; set; } = [];
    }
}
