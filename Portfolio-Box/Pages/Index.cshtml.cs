using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Portfolio_Box.Extensions;
using Portfolio_Box.Models;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;

namespace Portfolio_Box.Pages
{
	public class IndexModel : PageModel
	{
		public readonly ISharedFileRepository SharedFileRepository;
		public readonly CookieHandler CookieHandler;
		public new readonly User User;
		public readonly Uri RedirectUri;
		public readonly Uri DashboardUri;

		public IndexModel(ISharedFileRepository sharedFileRepository, IConfiguration configuration, User user, CookieHandler cookieHandler)
		{
			SharedFileRepository = sharedFileRepository;
			User = user;
			CookieHandler = cookieHandler;

			var portfolioUri = new Uri(configuration.GetValue<string>("Hosting:Portfolio")!);
			RedirectUri = portfolioUri.Append(configuration.GetValue<string>("Hosting:Redirect")!);
			DashboardUri = portfolioUri.Append(configuration.GetValue<string>("Hosting:Dashboard")!);
		}

		public IActionResult OnGet()
		{
			return User is AnonymousUser ? Redirect(RedirectUri.ToString()) : Page();
		}

		public IActionResult OnPostDisconnect()
		{
			CookieHandler.KillAll(HttpContext.Response);
			return Redirect(RedirectUri.ToString());
		}

		public IActionResult OnGetDashboard()
		{
			return Redirect(DashboardUri.ToString());
		}

		public PartialViewResult OnGetFileListPartial()
		{
			return new PartialViewResult()
			{
				ViewName = "_FileList",
				ViewData = new ViewDataDictionary<ISharedFileRepository>(ViewData, SharedFileRepository)
			};
		}
	}
}
