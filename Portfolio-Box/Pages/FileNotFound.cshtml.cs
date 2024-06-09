using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Pages
{
    public class FileNotFoundModel : PageModel
    {
        public new readonly User User;
        public FileNotFoundModel(User user)
        {
            User = user;
        }
    }
}
