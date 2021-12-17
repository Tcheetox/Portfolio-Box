using Microsoft.AspNetCore.Hosting;
using Portfolio_Box.Models.Shared;

namespace Portfolio_Box.Extensions
{
    public static class SharedFileExtension
    {
        public static string? BasePath { get; set; }

        public static string GetSize(this SharedFile sharedFile)
        {
            double gb = 1073741824;
            if (sharedFile.Length > gb)
                return (sharedFile.Length / gb).ToString("0.## GB"); // Display as Gigabytes
            else
                return (sharedFile.Length / (double)1048576).ToString("0.## MB"); // Display as Megabytes
        }

        public static string GetIcon(this SharedFile sharedFile)
        {
            string file;
            switch (sharedFile.Extension.ToLowerInvariant())
            {
                case ".xls":
                case ".xlsx":
                    file = "xls.png";
                    break;

                case ".exe":
                    file = "exe.png";
                    break;

                case ".mp3":
                    file = "mp3.png";
                    break;

                case ".wav":
                    file = "wav.png";
                    break;

                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".bmp":
                case ".tiff":
                    file = "picture.png";
                    break;

                case ".txt":
                    file = "txt.png";
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

            return BasePath + "/media/" + file;
        }
    }
}
