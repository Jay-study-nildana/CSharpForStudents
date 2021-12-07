using Microsoft.EntityFrameworkCore;

namespace DotNet6APIEFCoreSQLite.Models
{
    public class KaijuDBContext : DbContext
    {
        public KaijuDBContext(DbContextOptions<KaijuDBContext> options) : base(options)
        {
        }

        public DbSet<Kaiju> Kaijus { get; set; }
        //public DbSet<Enrollment> Enrollments { get; set; }
        //public DbSet<Student> Students { get; set; }

        //if you want, you can override the database names with the following code
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Kaiju>().ToTable("Kaiju Table");
        //    //modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        //    //modelBuilder.Entity<Student>().ToTable("Student");
        //}
    }



}
