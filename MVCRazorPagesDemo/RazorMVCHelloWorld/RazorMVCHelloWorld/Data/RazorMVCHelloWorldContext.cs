#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorMVCHelloWorld.Models;

namespace RazorMVCHelloWorld.Data
{
    public class RazorMVCHelloWorldContext : DbContext
    {
        public RazorMVCHelloWorldContext (DbContextOptions<RazorMVCHelloWorldContext> options)
            : base(options)
        {
        }

        public DbSet<RazorMVCHelloWorld.Models.Movie> Movie { get; set; }
    }
}
