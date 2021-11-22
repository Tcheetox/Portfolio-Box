using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public List<SharedLink> Links { get; set; }

        public SharedFile(int userId, string diskPath, string originalName, long length)
        {
            UserId = userId;
            DiskPath = diskPath;
            OriginalName = originalName;
            Length = length;
            UploadedOn = DateTime.Now;
            Links = new List<SharedLink>();
        }
    }
}
