using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Portfolio_Box.Extensions;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Controllers;

public abstract class ControllerBase : Controller
{
	private readonly IConfiguration _configuration;

	protected ControllerBase(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	protected ViewDataDictionary<File> GetViewData(File? file)
	{
		return new ViewDataDictionary<File>(ViewData, file)
		{
			["MediaBasePath"] = _configuration.GetMediaBasePath()
		};
	}
}