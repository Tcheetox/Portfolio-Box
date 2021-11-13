﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio_Box.Models;

namespace Portfolio_Box.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20211113093400_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Portfolio_Box.Models.Shared.SharedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("AuthorizedUserId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Size")
                        .HasColumnType("double");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UploadedOn")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorizedUserId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Shared.SharedLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime");

                    b.Property<string>("ExternalPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SharedFileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SharedFileId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnName("access_token")
                        .HasColumnType("text");

                    b.Property<uint>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.ToTable("oauth_tokens");
                });

            modelBuilder.Entity("Portfolio_Box.Models.User.AuthorizedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Shared.SharedFile", b =>
                {
                    b.HasOne("Portfolio_Box.Models.User.AuthorizedUser", null)
                        .WithMany("Files")
                        .HasForeignKey("AuthorizedUserId");
                });

            modelBuilder.Entity("Portfolio_Box.Models.Shared.SharedLink", b =>
                {
                    b.HasOne("Portfolio_Box.Models.Shared.SharedFile", null)
                        .WithMany("Links")
                        .HasForeignKey("SharedFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
