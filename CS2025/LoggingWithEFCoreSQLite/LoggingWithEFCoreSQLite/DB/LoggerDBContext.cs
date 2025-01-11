using Microsoft.EntityFrameworkCore;

namespace LoggingWithEFCoreSQLite.DB
{
    public class LoggerDBContext : DbContext
    {
        public LoggerDBContext(DbContextOptions<LoggerDBContext> options)
            : base(options)
        {
        }

        public DbSet<LogItem> LogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogItem>().HasKey(l => l.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
