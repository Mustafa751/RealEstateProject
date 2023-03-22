using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<Estate>().HasMany(e => e.Images).WithOne(e => e.Estate);
            modelBuilder.Entity<Estate>().HasOne(e => e.MainImage);
            modelBuilder.Entity<Image>().HasKey(e => e.Id);
            modelBuilder.Entity<Image>().HasOne(e => e.Estate).WithMany(e => e.Images);
        }
    }
}
