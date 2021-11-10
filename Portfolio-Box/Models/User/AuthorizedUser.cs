using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Portfolio_Box.Models.User
{
    public class AuthorizedUser : User
    {
        public override string Nickname { get; set; }
        public override string Email { get; set; }
        public override int Id { get; set; }

        public override void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
