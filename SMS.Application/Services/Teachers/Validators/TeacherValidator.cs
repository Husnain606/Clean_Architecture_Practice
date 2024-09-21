using FluentValidation;
using SMS.Application.Services.Teachers.Dto;

namespace SMS.Application.Services.Teachers.Validators
{
    public class TeacherValidator : AbstractValidator<CreateTeacherDto>
    {
        public TeacherValidator()
        {
            RuleFor(t => t.TeacherFirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters");

            RuleFor(t => t.TeacherLastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters");

            RuleFor(t => t.TeacherFatherName)
                .NotEmpty().WithMessage("Father Name is required")
                .MaximumLength(50).WithMessage("Father Name cannot exceed 50 characters");

            RuleFor(t => t.Age)
                .GreaterThan(20).WithMessage("Age must be greater than 20");

            RuleFor(t => t.CNIC)
                .NotEmpty().WithMessage("CNIC is required")
                .Length(13).WithMessage("CNIC must be exactly 13 digits");

            RuleFor(t => t.Mail)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email format");

            RuleFor(t => t.Pasword)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

            RuleFor(t => t.ConfirmPasword)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Equal(t => t.Pasword).WithMessage("Passwords do not match");

            RuleFor(t => t.SchoolId)
                .NotEmpty().WithMessage("School ID is required");

            RuleFor(t => t.BranchId)
                .NotEmpty().WithMessage("Branch ID is required");
        }
    }
}
