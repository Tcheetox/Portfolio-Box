namespace Portfolio_Box.Models.User
{
    public class AnonymousUser : User
    {
        public override string Nickname { get; set; } = "Anonymous";

        public override string Email { get; set; } = "Anonymous";

        public override int Id { get; set; } = -1;

        public override void Logout()
        {
            // Null user cannot logout
        }
    }
}
