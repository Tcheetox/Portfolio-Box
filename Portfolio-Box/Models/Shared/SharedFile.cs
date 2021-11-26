using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Portfolio_Box.Models.Shared
{
    public class SharedFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DiskPath { get; set; }
        public string OriginalName { get; set; }
        public long Length { get; set; }
        public DateTime UploadedOn { get; set; }
        public SharedLink? Link { get; set; }

        [NotMapped]
        public string Extension => Path.GetExtension(OriginalName);

        public SharedFile(int userId, string diskPath, string originalName, long length)
        {
            UserId = userId;
            DiskPath = diskPath;
            OriginalName = originalName;
            Length = length;
            UploadedOn = DateTime.Now;
        }
    }
}
