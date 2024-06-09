using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portfolio_Box.Models.Files;

namespace Portfolio_Box.Models.Links
{
    public class Link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        [ForeignKey("FileId")]
        public File? File { get; init; }
        public int FileId { get; init; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? DownloadUri { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime UpdatedOn { get; set; }

        [NotMapped]
        public bool IsExpired => DateTime.Now > Expiration;

        [NotMapped]
        public ExpiresIn ExpiryOption { get; init; }

        public enum ExpiresIn
        {
            [Display(Name = "One hour")]
            OneHour = 1,
            [Display(Name = "One day")]
            OneDay = 2,
            [Display(Name = "One week")]
            OneWeek = 3,
            [Display(Name = "One month")]
            OneMonth = 4,
            [Display(Name = "Never")]
            Never = 5
        }

        public Link(int fileId, string downloadUri, DateTime expiration, DateTime updatedOn)
        {
            FileId = fileId;
            DownloadUri = downloadUri;
            Expiration = expiration;
            UpdatedOn = updatedOn;
        }

        public Link(File file, ExpiresIn expiryOption)
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

        public void UpdateFrom(Link sharedLink)
        {
            DownloadUri = sharedLink.DownloadUri;
            Expiration = sharedLink.Expiration;
            UpdatedOn = sharedLink.UpdatedOn;
        }
    }
}
