using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, User user, IConfiguration configuration)
        {
            _logger = logger;
            _user = user;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (_user is AnonymousUser)
                return Redirect(_configuration.GetValue<string>("Hosting:Redirect"));
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