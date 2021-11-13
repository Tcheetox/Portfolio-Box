using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Box.Models.Shared
{
    public class SharedFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } 
        public string Path { get; set; }
        public string Author { get; set; }
        public double Size { get; set; }
        public DateTime UploadedOn { get; set; }
        public List<SharedLink> Links { get; set; }

        [NotMapped]
        public string Extension => System.IO.Path.GetExtension(Path);
    }
}
