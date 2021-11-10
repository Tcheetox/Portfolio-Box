using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio_Box.Models;
using Portfolio_Box.Models.User;
using System.Diagnostics;

namespace Portfolio_Box.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly User _user;

        public HomeController(ILogger<HomeController> logger, User user)
        {
            _logger = logger;
            _user = user;
        }

        public IActionResult Index()
        {
            if (_user is AnonymousUser)
                return Redirect("https://google.com");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}