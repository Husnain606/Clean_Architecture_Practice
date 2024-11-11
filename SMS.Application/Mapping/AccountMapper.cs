using AutoMapper;
using SMS.Application.Services.Account.Dto;
using SMS.Domain.Entities;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<CreateUserDto, ApplicationUser>()
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)) // Default value
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) // Set CreatedAt to current time
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); // Set ModifiedAt to current time
    }
}
