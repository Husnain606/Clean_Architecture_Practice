using Microsoft.AspNetCore.Identity;
using SMS.Domain.Entities;
using SMS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Presistence.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure the database is created
            await context.Database.EnsureCreatedAsync();

            // Seed roles
            if (!await roleManager.RoleExistsAsync("Student"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Student", NormalizedName = "STUDENT" });
            }
            if (!await roleManager.RoleExistsAsync("Teacher"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Teacher", NormalizedName = "TEACHER" });
            }

            // Seed an admin user
            var adminUser = new ApplicationUser
            {
                UserName = "HusnainAhmed",
                NormalizedUserName = "HUSNAINAHMED",
                Email = "hasnain606@gmail.com",
                NormalizedEmail = "HASNAIN606@GMAIL.COM",
                EmailConfirmed = true
            };

            // Check if the admin user already exists
            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, "NANI@606");
                await userManager.AddToRoleAsync(adminUser, "Student"); // Assign a role
            }
        }
    }
}
