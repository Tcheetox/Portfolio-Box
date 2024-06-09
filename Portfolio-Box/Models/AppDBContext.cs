using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.Files;
using Portfolio_Box.Models.Links;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Link> Links { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<AuthorizedUser>("AuthorizedUser")
                .HasValue<AnonymousUser>("AnonymousUser")
                .HasValue<AdminUser>("AdminUser");
            modelBuilder.Entity<Link>(l => l.HasIndex(i => i.DownloadUri).IsUnique());
            base.OnModelCreating(modelBuilder);
        }
    }
}
