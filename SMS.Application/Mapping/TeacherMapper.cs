using AutoMapper;
using SMS.Application.Services.Teachers.Dto;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public class TeacherMapper : Profile
    {
        public TeacherMapper()
        {
            // Mapping from CreateTeacherDto to Teacher and vice versa
            CreateMap<CreateTeacherDto, Teacher>()
                .ForMember(dest => dest.HiringDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();

            // Mapping from Teacher to TeacherDto and vice versa
            CreateMap<Teacher, TeacherDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.timespann, opt => opt.MapFrom(src => (DateTime.Now - src.HiringDate)))
                .AddTransform<string>(n => string.IsNullOrEmpty(n) ? "Class not Found" : n)
                .ReverseMap();
        }
    }
}
