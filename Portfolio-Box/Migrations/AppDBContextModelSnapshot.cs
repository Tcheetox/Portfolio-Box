﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio_Box.Models;

#nullable disable

namespace Portfolio_Box.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Portfolio_Box.Models.Files.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DiskPath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Remote")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("UploadedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Links.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DownloadUri")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("DownloadUri")
                        .IsUnique();

                    b.HasIndex("FileId")
                        .IsUnique();

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessToken")
                        .HasColumnType("longtext")
                        .HasColumnName("access_token");

                    b.Property<DateTime>("AccessTokenExpiresAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("access_token_expires_at");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("oauth_tokens");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("varchar(21)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<string>("Nickname")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Portfolio_Box.Models.Users.AnonymousUser", b =>
                {
                    b.HasBaseType("Portfolio_Box.Models.Users.User");

                    b.ToTable("users");

                    b.HasDiscriminator().HasValue("AnonymousUser");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Users.AuthorizedUser", b =>
                {
                    b.HasBaseType("Portfolio_Box.Models.Users.User");

                    b.ToTable("users");

                    b.HasDiscriminator().HasValue("AuthorizedUser");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Users.AdminUser", b =>
                {
                    b.HasBaseType("Portfolio_Box.Models.Users.AuthorizedUser");

                    b.ToTable("users");

                    b.HasDiscriminator().HasValue("AdminUser");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Files.File", b =>
                {
                    b.HasOne("Portfolio_Box.Models.Users.User", null)
                        .WithMany("Files")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Portfolio_Box.Models.Links.Link", b =>
                {
                    b.HasOne("Portfolio_Box.Models.Files.File", "File")
                        .WithOne("Link")
                        .HasForeignKey("Portfolio_Box.Models.Links.Link", "FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Files.File", b =>
                {
                    b.Navigation("Link");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Users.User", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
