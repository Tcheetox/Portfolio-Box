using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.Files;
using Portfolio_Box.Models.Links;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorizedUser>();
            modelBuilder.Entity<AnonymousUser>();
            modelBuilder.Entity<Link>(l => l.HasIndex(i => i.DownloadUri).IsUnique());

            base.OnModelCreating(modelBuilder);
        }
    }
}
