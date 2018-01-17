
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using TransportApp.Models;
using System;

namespace TransportApp.DbCore
{
    public class RepoContext: DbContext
    {
        public RepoContext(DbContextOptions<RepoContext> options) : base(options)
        {
        }

        public DbSet<TestTable> TestTableRepo { get; set; }
        
        public DbSet<Orders> Orders { get; set; }
        
        public DbSet<Vehicle> Vehicle { get; set; }

        public DbSet<Driver> Driver { get; set; }

        public DbSet<PriceList> PriceList { get; set; }

        public DbSet<Distance> Distance { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
           
                modelBuilder.Entity<Orders>().ToTable("Orders");
                modelBuilder.Entity<Orders>().HasKey(x => x.Id);

                modelBuilder.Entity<Distance>().ToTable("Distance");
                modelBuilder.Entity<Distance>().HasKey(x => x.Id);


                /*
                modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
                modelBuilder.Entity<Vehicle>().HasKey(x => x.Id);
                modelBuilder.Entity<Driver>().ToTable("Driver").HasKey(x => x.Id);  
                modelBuilder.Entity<PriceList>().ToTable("PriceList").HasKey(x => x.Id);*/
            }
            catch (Exception ex)
            {
                Console.Write("bad db definition");
            }

        }

    }
}
