using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Portfolio_Box.Extensions;
using Portfolio_Box.Models;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;
using Portfolio_Box.ViewModels;

namespace Portfolio_Box.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CookieHandler _cookieHandler;
        private readonly User _user;
        private readonly ISharedFileRepository _sharedFileRepository;
        private readonly Uri _redirectUri;
        private readonly Uri _dashboardUri;

        public HomeController(ILogger<HomeController> logger, User user, CookieHandler cookieHandler, ISharedFileRepository sharedFileRepository, IConfiguration configuration)
        {
            _logger = logger;
            _user = user;
            _sharedFileRepository = sharedFileRepository;
            _cookieHandler = cookieHandler;

            var portfolioUri = new Uri(configuration.GetValue<string>("Hosting:Portfolio"));
            _redirectUri = portfolioUri.Append(configuration.GetValue<string>("Hosting:Redirect"));
            _dashboardUri = portfolioUri.Append(configuration.GetValue<string>("Hosting:Dashboard"));
        }

        public IActionResult Index()
        {
            //if (_user is AnonymousUser)
            // return Redirect(_redirectURL);
            return View(_sharedFileRepository);
        }

        public RedirectResult Disconnect()
        {
            _cookieHandler.KillAll(HttpContext.Response);
            return Redirect(_redirectUri.ToString());
        }

        public RedirectResult Dashboard()
        {
            return Redirect(_dashboardUri.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}