using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            //empty for now.
        }

        //creating a table of comic books
        public DbSet<ComicBook> ComicBooks { get; set; }

        //seeding database
        //also neccessary for later DB operations and changing stuff
        //part of basic config
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //adding some default rows of data to the database.

            modelBuilder.Entity<ComicBook>().HasData(new ComicBook
            {
                ComicBookId = 1,
                ComicBookTitle = "Batman: Hush",
                ComicBookYearOfRelease = 2019,
                ComicBookISBN = "1401297242"
            });


            modelBuilder.Entity<ComicBook>().HasData(new ComicBook
            {
                ComicBookId = 2,
                ComicBookTitle = "Batman: Dark Victory",
                ComicBookYearOfRelease = 2014,
                ComicBookISBN = "1401244017"
            });
        }
    }
}
