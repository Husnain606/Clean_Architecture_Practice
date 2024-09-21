using FluentValidation;

namespace SMS.Application.Services.UserConfigurations.Dto
{
    public class UpdateUserConfigurationDtoValidator : AbstractValidator<UpdateUserConfigurationDto>
    {
        public UpdateUserConfigurationDtoValidator()
        {
            RuleFor(x => x.Id).Must(Validate).WithMessage("ID must not be an empty GUID.");
            RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(256);
            RuleFor(x => x.DisplayOrder).GreaterThan(0);
        }

        // This is the Validate method with the correct signature
        private bool Validate(Guid id)
        {
            return id != Guid.Empty; // Example: ensure the ID is valid
        }
    }
}
