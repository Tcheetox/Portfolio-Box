using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio_Box.Models.Shared;
using System;
using static Portfolio_Box.Models.Shared.SharedLink;

namespace Portfolio_Box.Controllers
{
    public class LinkController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly ISharedLinkRepository _sharedLinkRepository;
        private readonly ISharedFileRepository _sharedFileRepository;

        public LinkController(ILogger<FileController> logger, ISharedLinkRepository sharedLinkRepository, ISharedFileRepository sharedFileRepository)
        {
            _logger = logger;
            _sharedLinkRepository = sharedLinkRepository;
            _sharedFileRepository = sharedFileRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {
            if (form.TryGetValue("Link.Id", out var idString) && int.TryParse(idString, out int id)
                && form.TryGetValue("Link.ExpiryOption", out var expiryOptionString) && Enum.TryParse(expiryOptionString, out ExpiresIn expiryOption))
            {
                SharedFile targetFile = _sharedFileRepository.GetFileById(id);
                if (targetFile != null)
                {
                    _sharedLinkRepository.SaveLink(new SharedLink(targetFile, expiryOption));
                    return StatusCode(201);
                }
            }

            return BadRequest();
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sharedLinkRepository.DeleteLinkById(id);
            return NoContent();
        }
    }
}
