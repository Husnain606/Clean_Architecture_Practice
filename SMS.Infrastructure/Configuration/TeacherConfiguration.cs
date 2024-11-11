using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMS.Presistence.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.FatherName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Age)
                .IsRequired();

            builder.Property(t => t.CNIC)
                .IsRequired()
                .HasMaxLength(13);

            builder.Property(t => t.Mail)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(t => t.Mail)
                .IsUnique();

            //builder.Property(t => t.Pasword)
            //    .IsRequired()
            //    .HasMaxLength(100);

            //builder.Property(t => t.ConfirmPasword)
            //    .IsRequired()
            //    .HasMaxLength(100);

            builder.Property(t => t.SchoolId)
                .IsRequired();

            builder.Property(t => t.BranchId)
                .IsRequired();
        }
    }
}
