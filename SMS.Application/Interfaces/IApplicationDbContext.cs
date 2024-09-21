using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SMS.Domain.Entities;

namespace SMS.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Department> Department { get; set; }
        DbSet<Student> Student { get; set; }
        DbSet<Teacher> Teacher { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}
    