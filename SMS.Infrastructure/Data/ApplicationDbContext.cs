using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;
using SMS.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SMS.Presistence.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
            //Entities
            modelBuilder.ApplyConfiguration<Student>(new StudentConfiguration());
            modelBuilder.ApplyConfiguration<Department>(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration<Teacher>(new TeacherConfiguration());
           base.OnModelCreating(modelBuilder);
        }
        // Use the new keyword to hide the base class member intentionally
       public new DatabaseFacade Database => base.Database;
      
    }
}
