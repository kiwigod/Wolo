using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HoneymoonShop.Models;

namespace HoneymoonShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Dress> Dress { get; set; }

        public DbSet<Feature> Feature { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<DressFeature>()
                .HasKey(f => new { f.DressID, f.FeatureID });

            builder.Entity<DressFeature>()
                .HasOne(df => df.Dress)
                .WithMany(d => d.DressFeatures)
                .HasForeignKey(df => df.DressID);

            builder.Entity<DressFeature>()
                .HasOne(df => df.Feature)
                .WithMany(f => f.DressFeatures)
                .HasForeignKey(df => df.FeatureID);

            builder.Entity<DressColor>()
                .HasKey(c => new { c.DressID, c.ColorID });

            builder.Entity<DressColor>()
                .HasOne(dc => dc.Dress)
                .WithMany(d => d.DressColors)
                .HasForeignKey(dc => dc.DressID);

            builder.Entity<DressColor>()
                .HasOne(dc => dc.Color)
                .WithMany(c => c.DressColors)
                .HasForeignKey(dc => dc.ColorID);
        }

        public DbSet<DressFeature> DressFeature { get; set; }

        public DbSet<Color> Color { get; set; }

        public DbSet<Manu> Manu { get; set; }

        public DbSet<Neckline> Neckline { get; set; }

        public DbSet<Silhouette> Silhouette { get; set; }

        public DbSet<Style> Style { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<DressColor> DressColor { get; set; }

        public DbSet<Review> Review { get; set; }

        public DbSet<Appointment> Appointment { get; set; }

        public DbSet<Blog> Blog { get; set; }

        public DbSet<Post> Post { get; set; }
    }
}
