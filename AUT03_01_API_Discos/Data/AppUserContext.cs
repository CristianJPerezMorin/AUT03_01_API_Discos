using AUT03_01_API_Discos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AUT03_01_API_Discos.Data
{
    public partial class AppUserContext : IdentityDbContext<AppUser>
    {

        public AppUserContext(DbContextOptions<AppUserContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Album>().ToTable(nameof(Artist), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Artist>().ToTable(nameof(Album), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Track>().ToTable(nameof(Track), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<AppUser>().Ignore( t => t.roles);


            modelBuilder.Entity<AppUser>(b =>
            {
                b.ToTable(nameof(AppUser));
                b.Property(u => u.Nombre).HasMaxLength(20);
                b.Property(u => u.Apellidos).HasMaxLength(40);
                b.Property(u => u.CodPostal).HasDefaultValue(00000);
            });

            
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Default",
                    NormalizedName = "DEFAULT"
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);

            List<AppUser> users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "Admin@api.com",
                    Email = "Admin@api.com",
                    NormalizedEmail = "ADMIN@API.COM",
                    NormalizedUserName = "ADMIN@API.COM",
                    EmailConfirmed = true,
                    Nombre = "Cristian",
                    Apellidos = "Admin Admin",
                    CodPostal = 12345,
                },
                new AppUser
                {
                    UserName = "Manager@api.com",
                    Email = "Manager@api.com",
                    NormalizedEmail = "MANAGER@API.COM",
                    NormalizedUserName = "MANAGER@API.COM",
                    EmailConfirmed = true,
                    Nombre = "Manager",
                    Apellidos = "Manager Manager",
                    CodPostal = 54321
                }
            };
            modelBuilder.Entity<AppUser>().HasData(users);
            var passwordHasher = new PasswordHasher<IdentityUser>();
            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "adminpass");
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], "manapass");

            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = roles.Find(r => r.Name == "Admin").Id,
                    UserId = users[0].Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = roles.Find(r => r.Name == "Manager").Id,
                    UserId = users[1].Id
                }
            };

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
            
        }


    }
}
