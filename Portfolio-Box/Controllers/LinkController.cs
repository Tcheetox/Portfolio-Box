using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Portfolio_Box.Models.Shared;
using static Portfolio_Box.Models.Shared.SharedLink;

namespace Portfolio_Box.Controllers
{
    public class LinkController : Controller
    {
        private readonly ISharedLinkRepository _sharedLinkRepository;
        private readonly ISharedFileRepository _sharedFileRepository;

        public LinkController(ISharedLinkRepository sharedLinkRepository, ISharedFileRepository sharedFileRepository)
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
                    _sharedLinkRepository.SaveLink(new SharedLink(targetFile, expiresIn));
                    return new PartialViewResult()
                    {
                        ViewName = "_FileDetails",
                        ViewData = new ViewDataDictionary<SharedFile>(ViewData, targetFile),
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
                ViewData = new ViewDataDictionary<SharedFile>(ViewData, file),
            };
        }
    }
}
