using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HoneymoonShop.Data;

namespace HoneymoonShop.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170110221535_review")]
    partial class review
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HoneymoonShop.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Appointment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("MDate");

                    b.Property<string>("Mail");

                    b.Property<string>("Name");

                    b.Property<string>("PNumber");

                    b.Property<bool>("newsletter");

                    b.HasKey("ID");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Color", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColorCode");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Color");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Dress", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("Description");

                    b.Property<int>("ManuID");

                    b.Property<int>("NecklineID");

                    b.Property<int>("Price");

                    b.Property<int>("SilhouetteID");

                    b.Property<int>("StyleID");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("ManuID");

                    b.HasIndex("NecklineID");

                    b.HasIndex("SilhouetteID");

                    b.HasIndex("StyleID");

                    b.ToTable("Dress");
                });

            modelBuilder.Entity("HoneymoonShop.Models.DressColor", b =>
                {
                    b.Property<int>("DressID");

                    b.Property<int>("ColorID");

                    b.HasKey("DressID", "ColorID");

                    b.HasIndex("ColorID");

                    b.HasIndex("DressID");

                    b.ToTable("DressColor");
                });

            modelBuilder.Entity("HoneymoonShop.Models.DressFeature", b =>
                {
                    b.Property<int>("DressID");

                    b.Property<int>("FeatureID");

                    b.HasKey("DressID", "FeatureID");

                    b.HasIndex("DressID");

                    b.HasIndex("FeatureID");

                    b.ToTable("DressFeature");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Feature", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Feature");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Manu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Manu");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Neckline", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Neckline");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Review", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("Rating");

                    b.HasKey("ID");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Silhouette", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Silhouette");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Style", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Style");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HoneymoonShop.Models.Dress", b =>
                {
                    b.HasOne("HoneymoonShop.Models.Category", "Category")
                        .WithMany("Dresses")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HoneymoonShop.Models.Manu", "Manu")
                        .WithMany("Dresses")
                        .HasForeignKey("ManuID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HoneymoonShop.Models.Neckline", "Neckline")
                        .WithMany("Dresses")
                        .HasForeignKey("NecklineID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HoneymoonShop.Models.Silhouette", "Silhouette")
                        .WithMany("Dresses")
                        .HasForeignKey("SilhouetteID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HoneymoonShop.Models.Style", "Style")
                        .WithMany("Dresses")
                        .HasForeignKey("StyleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HoneymoonShop.Models.DressColor", b =>
                {
                    b.HasOne("HoneymoonShop.Models.Color", "Color")
                        .WithMany("DressColors")
                        .HasForeignKey("ColorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HoneymoonShop.Models.Dress", "Dress")
                        .WithMany("DressColors")
                        .HasForeignKey("DressID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HoneymoonShop.Models.DressFeature", b =>
                {
                    b.HasOne("HoneymoonShop.Models.Dress", "Dress")
                        .WithMany("DressFeatures")
                        .HasForeignKey("DressID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HoneymoonShop.Models.Feature", "Feature")
                        .WithMany("DressFeatures")
                        .HasForeignKey("FeatureID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HoneymoonShop.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HoneymoonShop.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HoneymoonShop.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
