using Portfolio_Box.Models.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Box.Models.User
{
    [NotMapped]
    public class AnonymousUser : User
    {
        public override string Nickname { get; set; } = "Anonymous";
        public override string Email { get; set; } = "Anonymous";
        public override int Id { get; set; } = -1;
        public override List<SharedFile> Files { get; set; } = new List<SharedFile>();

        public override void Logout()
        {
            // Null user cannot logout
        }
    }
}
