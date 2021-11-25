using Portfolio_Box.Models.Shared;
using System.IO;

namespace Portfolio_Box.Extensions
{
    public static class SharedFileExtension
    {
        public static string GetSize(this SharedFile sharedFile)
        {   
            int gb = 1073741824;
            if (sharedFile.Length < gb)
            {
                // Display as Gigabytes
                return (sharedFile.Length / gb).ToString("#.##");
            }
            else
            {
                // Display as Megabytes
                return (sharedFile.Length / 1048576).ToString("#.##");
            }
        }

        public static string GetIcon(this SharedFile sharedFile)
        {
            string extension = Path.GetExtension(sharedFile.OriginalName);
            string file;
            switch (extension)
            {
                case ".xls":
                case ".xlsx":
                    file = "xls.png";
                    break;

                case ".pdf":
                    file = "pdf.png";
                    break;

                case ".ppt":
                case ".pptx":
                    file = "ppt.png";
                    break;

                case ".avi":
                case ".mp4":
                case ".mpg":
                case ".mpeg":
                case ".mkv":
                    file = "vlc.png";
                    break;

                case ".doc":
                case ".docx":
                    file = "word.png";
                    break;

                case ".zip":
                case ".rar":
                    file = "zip.png";
                    break;

                default:
                    file = "file.png";
                    break;
            }

            return "~/media/" + file;
        }
    }
}
