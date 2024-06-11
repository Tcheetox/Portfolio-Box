using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Portfolio_Box.Models.Links;

namespace Portfolio_Box.Models.Files
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        public int UserId { get; init; }
        public string DiskPath { get; set; }
        public string OriginalName { get; set; }
        public long Length { get; set; }
        public DateTime UploadedOn { get; init; } = DateTime.Now;
        public Link? Link { get; init; }
        public bool Remote { get; init; }

        [NotMapped]
        public string Extension => Path.GetExtension(OriginalName);

        [NotMapped]
        public string Size
        {
            get
            {
                double gb = 1073741824;
                if (Length > gb)
                    return (Length / gb).ToString("0.## GB"); // Display as Gigabytes
                return (Length / (double)1048576).ToString("0.## MB"); // Display as Megabytes
            }
        }

        [NotMapped]
        public string Icon => Extension.ToLowerInvariant() switch
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

        public File(int userId, string diskPath, string originalName, long length, bool remote = false)
        {
            UserId = userId;
            DiskPath = diskPath;
            OriginalName = originalName;
            Length = length;
            Remote = remote;
        }
    }
}
