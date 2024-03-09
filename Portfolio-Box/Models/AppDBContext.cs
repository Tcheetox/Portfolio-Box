﻿using Microsoft.EntityFrameworkCore;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;

namespace Portfolio_Box.Models
{
	public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
	{
		public DbSet<User.User> Users { get; set; }
		public DbSet<Token> Tokens { get; set; }
		public DbSet<SharedFile> Files { get; set; }
		public DbSet<SharedLink> Links { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AuthorizedUser>();
			modelBuilder.Entity<AnonymousUser>();
			modelBuilder.Entity<SharedLink>(l => l.HasIndex(i => i.DownloadUri).IsUnique());

			base.OnModelCreating(modelBuilder);
		}
	}
}
