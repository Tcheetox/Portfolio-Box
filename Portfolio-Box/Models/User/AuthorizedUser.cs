using System.Collections.Generic;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Models.User
{
    public class AuthorizedUser : User
    {
        public override string? Nickname { get; set; } = "Loading...";
        public override string Email { get; set; } = "Loading...";
        public override int Id { get; set; }
        public override List<SharedFile> Files { get; set; } = [];
    }
}
