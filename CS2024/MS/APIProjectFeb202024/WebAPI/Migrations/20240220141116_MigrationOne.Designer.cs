﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPI.Data;

#nullable disable

namespace WebAPI.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240220141116_MigrationOne")]
    partial class MigrationOne
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("WebAPI.Models.ComicBook", b =>
                {
                    b.Property<int>("ComicBookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

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
                            ComicBookTitle = "Batman",
                            ComicBookYearOfRelease = 10
                        },
                        new
                        {
                            ComicBookId = 2,
                            ComicBookTitle = "10OFF",
                            ComicBookYearOfRelease = 10
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
