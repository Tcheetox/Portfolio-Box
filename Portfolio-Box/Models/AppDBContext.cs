using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.User;

namespace Portfolio_Box.Models
{
    public class AppDBContext : DbContext
    {

        public DbSet<AuthorizedUser> Users { get; set; }

        public DbSet<Token> Tokens { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AuthorizedUser>();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
