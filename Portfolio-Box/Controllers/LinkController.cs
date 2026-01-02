using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Portfolio_Box.Models.Files;
using Portfolio_Box.Models.Links;
using static Portfolio_Box.Models.Links.Link;

namespace Portfolio_Box.Controllers;

public class LinkController : ControllerBase
{
	private readonly IFileRepository _fileRepository;
	private readonly ILinkRepository _linkRepository;

	public LinkController(ILinkRepository linkRepository, IFileRepository fileRepository, IConfiguration configuration)
		: base(configuration)
	{
		_fileRepository = fileRepository;
		_linkRepository = linkRepository;
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Create(int id, string expiry)
	{
		if (Enum.TryParse(expiry, out ExpiresIn expiresIn))
		{
			var file = _fileRepository.GetFileById(id);
			if (file is not null)
			{
				_linkRepository.SaveLink(new Link(file, expiresIn));
				return new PartialViewResult
				{
					ViewName = "_FileDetails",
					ViewData = GetViewData(file),
					StatusCode = 201
				};
			}
		}

		return BadRequest();
	}

	[HttpDelete]
	[ValidateAntiForgeryToken]
	public IActionResult Delete(int id)
	{
		var file = _linkRepository.DeleteLinkById(id);
		if (file is null)
			return BadRequest();

		return new PartialViewResult
		{
			ViewName = "_FileDetails",
			ViewData = GetViewData(file)
		};
	}
}