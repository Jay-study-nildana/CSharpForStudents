using Mango.Services.AuthAPI;
using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mango.Services.AuthAPI.Data
{
    //TODO. this is currently working with SQLite as I wish to avoid using Microsoft SQL Server, local or online. 
    //Perhaps we can make it work with an SQL server.
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            //empty for now.
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //seeding database
        //also neccessary for later DB operations and changing stuff
        //part of basic config
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
