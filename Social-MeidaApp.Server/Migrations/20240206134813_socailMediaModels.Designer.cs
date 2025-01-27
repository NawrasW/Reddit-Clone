﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Social_MeidaApp.Server.Models.Domain;

#nullable disable

namespace Social_MeidaApp.Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240206134813_socailMediaModels")]
    partial class socailMediaModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<int>("AuthorUserId")
                        .HasColumnType("int");

                    //b.Property<int?>("CommentId1")
                    //    .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("AuthorUserId");

                    //b.HasIndex("CommentId1");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Like", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("CommentId")
                        .HasColumnType("int");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CommentId", "PostId");

                    b.HasIndex("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<int>("AuthorUserId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PostId");

                    b.HasIndex("AuthorUserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Comment", b =>
                {
                    b.HasOne("Social_MeidaApp.Server.Models.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    //b.HasOne("Social_MeidaApp.Server.Models.Comment", null)
                    //    .WithMany("Replies")
                    //    .HasForeignKey("CommentId1");

                    b.HasOne("Social_MeidaApp.Server.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Like", b =>
                {
                    b.HasOne("Social_MeidaApp.Server.Models.Comment", "Comment")
                        .WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Social_MeidaApp.Server.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Social_MeidaApp.Server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Post", b =>
                {
                    b.HasOne("Social_MeidaApp.Server.Models.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Comment", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Social_MeidaApp.Server.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
