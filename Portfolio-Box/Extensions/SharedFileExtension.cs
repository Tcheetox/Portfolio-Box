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
            string file = sharedFile.Extension.ToLowerInvariant() switch
            {
                ".xls" or ".xlsx" => "xls.png",
                ".exe" => "exe.png",
                ".mp3" => "mp3.png",
                ".wav" => "wav.png",
                ".jpg" or ".jpeg" or ".gif" or ".png" or ".bmp" or ".tiff" => "picture.png",
                ".txt" => "txt.png",
                ".pdf" => "pdf.png",
                ".ppt" or ".pptx" => "ppt.png",
                ".avi" or ".mp4" or ".mpg" or ".mpeg" or ".mkv" => "vlc.png",
                ".doc" or ".docx" => "word.png",
                ".zip" or ".rar" => "zip.png",
                _ => "file.png",
            };
            return BasePath + "/media/" + file;
        }
    }
}
