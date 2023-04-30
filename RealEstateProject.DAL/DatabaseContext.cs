using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RealEstateProject.DAL
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Estate> estates { get; set; }
        public virtual DbSet<Image> images { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estate>().HasKey(e => e.Id);
            modelBuilder.Entity<Estate>().Property(e => e.City).HasConversion(new EnumToStringConverter<City>());
            modelBuilder.Entity<Estate>().Property(e => e.EstateType).HasConversion(new EnumToStringConverter<Type>());
            modelBuilder.Entity<Estate>().HasMany(e => e.Images).WithOne(e => e.Estate);
            modelBuilder.Entity<Estate>().HasOne(e => e.MainImage).WithOne().HasForeignKey<Estate>(e => e.MainImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Image>().HasKey(e => e.Id);
            modelBuilder.Entity<Image>().HasOne(e => e.Estate).WithMany(e => e.Images);
        }
    }
}
