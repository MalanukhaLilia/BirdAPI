using BirdAPI_lab4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BirdAPI_lab4.Data
{
    public class BirdFarmDbContext : DbContext
    {
        public BirdFarmDbContext(DbContextOptions<BirdFarmDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bird> Birds { get; set; }
        public DbSet<Egg> Eggs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bird>()
                .HasMany(b => b.Eggs)
                .WithOne(e => e.Bird)
                .HasForeignKey(e => e.BirdId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}