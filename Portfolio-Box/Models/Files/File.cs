using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Portfolio_Box.Models.Links;

namespace Portfolio_Box.Models.Files
{
    public class File(int userId, string diskPath, string originalName, long length)
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; } = userId;
        public string DiskPath { get; set; } = diskPath;
        public string OriginalName { get; set; } = originalName;
        public long Length { get; set; } = length;
        public DateTime UploadedOn { get; set; } = DateTime.Now;
        public Link? Link { get; set; }

        [NotMapped]
        public string Extension => Path.GetExtension(OriginalName);
    }
}
