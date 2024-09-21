using AutoMapper;
using SMS.Application.Services.Teachers.Dto;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public class TeacherMapper : Profile
    {
        public TeacherMapper()
        {
            CreateMap<CreateTeacherDto,Teacher>().ReverseMap();
            CreateMap<TeacherDto, Teacher>().ReverseMap().ForMember(d => d.Name, s => s.MapFrom(std => std.TeacherFirstName + " " + std.TeacherLastName))
                .ForMember(d => d.timespann, s => s.MapFrom(std => (DateTime.Now - std.HiringDate)))
                .AddTransform<String>(n => string.IsNullOrEmpty(n) ? "Class not Found" : n);
            //   .ForMember(d => d.StudentLastName, s => s.Ignore())
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}
