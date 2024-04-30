using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mango.Services.CouponAPI.Data
{
    //TODO. this is currently working with SQLite as I wish to avoid using Microsoft SQL Server, local or online. 
    //Perhaps we can make it work with an SQL server.
    public class AppDBContext : DbContext
    {
        

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            //empty for now.
        }

        public DbSet<Coupon> Coupons { get; set; }

        //seeding database
        //also neccessary for later DB operations and changing stuff
        //part of basic config
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20
            });


            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "20OFF",
                DiscountAmount = 20,
                MinAmount = 40
            });
        }

    }
}
