using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Box.Models.Shared
{
    public class SharedLink
    {
        public int Id { get; set; }
        public int SharedFileId { get; set; }
        public string ExternalPath { get; set; }
        public DateTime Expiration { get; set; }

        [NotMapped]
        public bool IsExpired => DateTime.Now > Expiration;
    }
}
