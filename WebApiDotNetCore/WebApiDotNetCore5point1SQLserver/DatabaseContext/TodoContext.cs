using Microsoft.EntityFrameworkCore;
using WebApiDotNetCore5point1SQLserver.Models;

namespace WebApiDotNetCore5point1SQLite.DatabaseContext
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}