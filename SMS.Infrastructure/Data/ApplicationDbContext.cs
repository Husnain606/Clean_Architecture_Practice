using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;
using SMS.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SMS.Presistence.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SMS.Persistence.Configuration;

namespace SMS.Infrastructure.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser> , IApplicationDbContext
    {
        //Entities
        public DbSet<Department> Department { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }

        public ApplicationDbContext(DbContextOptions options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Entities
            modelBuilder.ApplyConfiguration<ApplicationUser>(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration<Student>(new StudentConfiguration());
            modelBuilder.ApplyConfiguration<Department>(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration<Teacher>(new TeacherConfiguration());
            base.OnModelCreating(modelBuilder);

            // Seed default Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Administration",
                    NormalizedName = "ADMINISTRATION"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // Seed default ApplicationUsers
            var adminUser = new ApplicationUser
            {
                Id = "1",
                UserName = "HusnainAhmed",
                NormalizedUserName = "HUSNAINAHMED",
                Email = "hasnain606@gmail.com",
                NormalizedEmail = "HASNAIN606@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "NANI@606")
            };

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign Admin role to the seeded Admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "1",
                    RoleId = "1" // Admin role Id
                }
            );
        }

        // Use the new keyword to hide the base class member intentionally
       public new DatabaseFacade Database => base.Database;
     

    }
}
