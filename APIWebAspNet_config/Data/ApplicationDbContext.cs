﻿using APIWebAspNet_config.Models;
using Microsoft.EntityFrameworkCore;

namespace APIWebAspNet_config.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon()
                {
                    Id = 1,
                    Name = "10OFF",
                    Percent = 10,
                    IsActive = true,
                },
                new Coupon()
                {
                    Id = 2,
                    Name = "20OFF",
                    Percent = 20,
                    IsActive = true,
                });
        }


        //Collegamento con DB: se non lo indichiamo in Program.cs =>
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
        // optionsBuilder.UseSqlServer(MyConnection); }

    }
}


