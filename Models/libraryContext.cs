using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace LibraryManagementAPI.Models;

public partial class libraryContext : DbContext
{
    public libraryContext()
    {
    }

    public libraryContext(DbContextOptions<libraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Borrow> Borrows { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Notice> Notices { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookID).HasName("PRIMARY");

            entity.HasIndex(e => e.CategoryID, "CategoryID");

            entity.Property(e => e.BookID).HasComment("图书ID");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .HasComment("作者");
            entity.Property(e => e.AvailabilityStatus)
                .HasMaxLength(50)
                .HasComment("可用状态");
            entity.Property(e => e.CategoryID).HasComment("分类ID");
            entity.Property(e => e.ISBN)
                .HasMaxLength(255)
                .HasComment("ISBN码");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasComment("图片地址");
            entity.Property(e => e.Introduction)
                .HasMaxLength(255)
                .HasComment("图书简介");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasComment("位置");
            entity.Property(e => e.Price)
                .HasMaxLength(50)
                .HasComment("价格");
            entity.Property(e => e.PublicationDate).HasComment("出版年份");
            entity.Property(e => e.PublishingHouse)
                .HasMaxLength(50)
                .HasComment("出版社");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasComment("标题");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Books_ibfk_1");
        });

        modelBuilder.Entity<Borrow>(entity =>
        {
            entity.HasKey(e => e.BorrowID).HasName("PRIMARY");

            entity.HasIndex(e => e.BookID, "BookID");

            entity.HasIndex(e => e.ReaderID, "ReaderID");

            entity.HasOne(d => d.Book).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.BookID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Borrows_ibfk_1");

            entity.HasOne(d => d.Reader).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.ReaderID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Borrows_ibfk_2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryID).HasName("PRIMARY");

            entity.Property(e => e.CategoryID).HasComment("分类ID");
            entity.Property(e => e.CategoryChar)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasComment("分类字符（中图法）");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasComment("分类名称");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.FineID).HasName("PRIMARY");

            entity.HasIndex(e => e.BorrowID, "BorrowID");

            entity.Property(e => e.FineAmount).HasPrecision(10, 2);

            entity.HasOne(d => d.Borrow).WithMany(p => p.Fines)
                .HasForeignKey(d => d.BorrowID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fines_ibfk_1");
        });

        modelBuilder.Entity<Notice>(entity =>
        {
            entity.HasKey(e => e.NoticeID).HasName("PRIMARY");

            entity.HasIndex(e => e.UserID, "UserID");

            entity.Property(e => e.NoticeID).HasComment("公告ID");
            entity.Property(e => e.Content)
                .HasComment("内容")
                .HasColumnType("text");
            entity.Property(e => e.CreationDate).HasComment("发布日期");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasComment("标题");
            entity.Property(e => e.UserID).HasComment("作者ID");

            entity.HasOne(d => d.User).WithMany(p => p.Notices)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notices_ibfk_1");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderID).HasName("PRIMARY");

            entity.Property(e => e.ReaderID).HasComment("读者ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasComment("邮箱");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasComment("姓名");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasComment("密码");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .HasComment("手机号");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PRIMARY");

            entity.Property(e => e.UserID).HasComment("用户ID");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasComment("密码");
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .HasComment("用户类型");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasComment("用户名");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
