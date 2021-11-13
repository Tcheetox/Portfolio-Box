using Microsoft.AspNetCore.Mvc;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Components
{
    public class FileTile : ViewComponent
    {
        private readonly SharedFile _sharedFile;
        public FileTile(SharedFile sharedFile)
        {
            _sharedFile = sharedFile;
        }

        public IViewComponentResult Invoke()
        {
            return View(_sharedFile);
        }
    }
}
