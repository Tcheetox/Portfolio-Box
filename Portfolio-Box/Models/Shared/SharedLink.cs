using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Box.Models.Shared
{
    public class SharedLink
    {
        [Key]
        public int Id { get; set; }
        public SharedFile File { get; set; }
        public int FileId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DownloadUri { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime UpdatedOn { get; set; }

        [NotMapped]
        public bool IsExpired => DateTime.Now > Expiration;

        [NotMapped]
        public ExpiresIn ExpiryOption { get; set; }

        public enum ExpiresIn
        {
            OneHour = 1,
            OneDay = 2,
            OneWeek = 3,
            OneMonth = 4,
            Never = 5
        }

        public SharedLink(int fileId, string downloadUri, DateTime expiration, DateTime updatedOn)
        {
            FileId = fileId;
            DownloadUri = downloadUri;
            Expiration = expiration;
            UpdatedOn = updatedOn;
        }

        public SharedLink(SharedFile file, ExpiresIn expiryOption)
        {
            FileId = file.Id;
            File = file;
            UpdatedOn = DateTime.Now;
            switch (expiryOption)
            {
                case ExpiresIn.OneHour:
                    Expiration = UpdatedOn.AddHours(1);
                    break;
                case ExpiresIn.OneDay:
                    Expiration = UpdatedOn.AddDays(1);
                    break;
                case ExpiresIn.OneWeek:
                    Expiration = UpdatedOn.AddDays(7);
                    break;
                case ExpiresIn.OneMonth:
                    Expiration = UpdatedOn.AddMonths(1);
                    break;
                case ExpiresIn.Never:
                    Expiration = DateTime.MaxValue;
                    break;
            }
        }

        public SharedLink UpdateFrom(SharedLink sharedLink)
        {
            DownloadUri = sharedLink.DownloadUri;
            Expiration = sharedLink.Expiration;
            UpdatedOn = sharedLink.UpdatedOn;
            return this;
        }
    }
}
