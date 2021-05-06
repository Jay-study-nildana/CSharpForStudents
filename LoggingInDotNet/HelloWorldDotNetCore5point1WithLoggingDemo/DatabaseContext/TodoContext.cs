using Microsoft.EntityFrameworkCore;
using HelloWorldDotNetCore5point1.Models;

namespace HelloWorldDotNetCore5point1.DatabaseContext
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