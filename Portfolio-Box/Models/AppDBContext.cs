using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;
using System.Collections.Generic;

namespace Portfolio_Box.Models
{
    public class AppDBContext : DbContext
    {
        public DbSet<User.User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<SharedFile> Files { get; set; }
        public DbSet<SharedLink> Links { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorizedUser>();
            modelBuilder.Entity<AnonymousUser>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
