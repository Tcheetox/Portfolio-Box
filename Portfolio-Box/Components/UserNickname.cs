using Microsoft.AspNetCore.Mvc;
using Portfolio_Box.Models.User;

namespace Portfolio_Box.Components
{
    public class UserNickname : ViewComponent
    {
        private readonly User _user;
        public UserNickname(User user)
        {
            _user = user;
        }

        public IViewComponentResult Invoke()
        {
            return View(_user);
        }
    }
}
