using Microsoft.AspNetCore.Mvc;
using Portfolio_Box.Models.User;

namespace Portfolio_Box.Components
{
    public class UserMenu : ViewComponent
    {
        private readonly User _user;
        public UserMenu(User user)
        {
            _user = user;
        }

        public IViewComponentResult Invoke()
        {
            return View(_user);
        }
    }
}
