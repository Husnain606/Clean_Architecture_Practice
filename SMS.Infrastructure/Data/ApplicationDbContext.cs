using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SMS.Domain.Entities;
using SMS.Application.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SMS.Persistence.Configuration;
using SMS.Presistence.Configuration;

namespace SMS.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Department> Department { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure Student Entity
            modelBuilder.Entity<Student>()
                .HasOne(s => s.ApplicationUser)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Student)
                .HasForeignKey(s => s.DepartmentId);

            // Configure Teacher Entity
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.ApplicationUser)
                .WithOne()
                .HasForeignKey<Teacher>(t => t.UserId);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());

            // Seed default roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole { Id = "2", Name = "Teacher", NormalizedName = "TEACHER" }
            );

            // Seed default ApplicationUser (Admin user)
            var adminUser = new ApplicationUser
            {
                Id = "1",
                UserName = "HusnainAhmed",
                NormalizedUserName = "HUSNAINAHMED",
                Email = "hasnain606@gmail.com",
                NormalizedEmail = "HASNAIN606@GMAIL.COM",
                EmailConfirmed = true
            };

            // Hash the password
            var hasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "NANI@606");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign Admin role to the seeded Admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "1", // Admin user Id
                    RoleId = "1"  // Admin role Id
                }
            );
        }

        // Expose the Database object for migrations
        public new DatabaseFacade Database => base.Database;
    }
}
