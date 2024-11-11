using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMS.Presistence.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(s => s.FatherName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(s => s.Age)
                .IsRequired();

            builder.Property(s => s.Class)
                .IsRequired();

            builder.Property(s => s.Contact)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(s => s.Mail)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(s => s.Mail)
                .IsUnique();

            

            builder.Property(s => s.DepartmentId)
                .IsRequired();

            builder.HasOne(s => s.Department)
                .WithMany(d => d.Student)
                .HasForeignKey(s => s.DepartmentId);
        }
    }
}