using LegoNPU.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LegoNPU.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Rating> Ratings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Image>().HasKey(i => i.Id);
            modelBuilder.Entity<Rating>().HasKey(r => r.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Images)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Image>()
                .HasMany(i => i.Ratings)
                .WithOne(r => r.Image)
                .HasForeignKey(r => r.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Rating>()
                .Property(r => r.Score)
                .IsRequired(); 
        }
    }
}
