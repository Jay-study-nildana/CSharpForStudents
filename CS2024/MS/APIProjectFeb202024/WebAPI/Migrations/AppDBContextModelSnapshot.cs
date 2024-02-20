﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPI.Data;

#nullable disable

namespace WebAPI.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("WebAPI.Models.ComicBook", b =>
                {
                    b.Property<int>("ComicBookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ComicBookISBN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ComicBookTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ComicBookYearOfRelease")
                        .HasColumnType("INTEGER");

                    b.HasKey("ComicBookId");

                    b.ToTable("ComicBooks");

                    b.HasData(
                        new
                        {
                            ComicBookId = 1,
                            ComicBookISBN = "1401297242",
                            ComicBookTitle = "Batman: Hush",
                            ComicBookYearOfRelease = 2019
                        },
                        new
                        {
                            ComicBookId = 2,
                            ComicBookISBN = "1401244017",
                            ComicBookTitle = "Batman: Dark Victory",
                            ComicBookYearOfRelease = 2014
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
