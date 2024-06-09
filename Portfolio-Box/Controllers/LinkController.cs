using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Portfolio_Box.Models.Files;
using Portfolio_Box.Models.Links;
using static Portfolio_Box.Models.Links.Link;

namespace Portfolio_Box.Controllers
{
    public class LinkController : Controller
    {
        private readonly ILinkRepository _sharedLinkRepository;
        private readonly IFileRepository _sharedFileRepository;

        public LinkController(ILinkRepository sharedLinkRepository, IFileRepository sharedFileRepository)
        {
            _sharedFileRepository = sharedFileRepository;
            _sharedLinkRepository = sharedLinkRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, string expiry)
        {
            if (Enum.TryParse(expiry, out ExpiresIn expiresIn))
            {
                var targetFile = _sharedFileRepository.GetFileById(id);
                if (targetFile is not null)
                {
                    _sharedLinkRepository.SaveLink(new Link(targetFile, expiresIn));
                    return new PartialViewResult()
                    {
                        ViewName = "_FileDetails",
                        ViewData = new ViewDataDictionary<File>(ViewData, targetFile),
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
            var file = _sharedLinkRepository.DeleteLinkById(id);
            if (file is null)
                return BadRequest();

            return new PartialViewResult()
            {
                ViewName = "_FileDetails",
                ViewData = new ViewDataDictionary<File>(ViewData, file),
            };
        }
    }
}
