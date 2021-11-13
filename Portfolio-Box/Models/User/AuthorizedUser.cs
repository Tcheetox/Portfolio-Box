using Portfolio_Box.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Box.Models.User
{
    [Table("users")]
    public class AuthorizedUser : User
    {
        public override string Nickname { get; set; } = "Loading...";
        public override string Email { get; set; } = "Loading...";
        public override int Id { get; set; }
        public override List<SharedFile> Files { get; set; } = new List<SharedFile>();

        public override void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
