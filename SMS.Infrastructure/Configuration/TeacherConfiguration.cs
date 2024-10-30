using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Presistence.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TeacherFirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.TeacherLastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.TeacherFatherName)
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
