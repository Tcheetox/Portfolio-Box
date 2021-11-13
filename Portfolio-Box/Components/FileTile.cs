using Microsoft.AspNetCore.Mvc;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Components
{
    public class FileTile : ViewComponent
    {
        public IViewComponentResult Invoke(SharedFile sharedFile)
        {
            return View(sharedFile);
        }
    }
}
