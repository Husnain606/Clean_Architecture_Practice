using AutoMapper;
using SMS.Application.Services.Students.Dto;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<CreateStudentDto, Student>().ReverseMap();
            CreateMap<StudentDto, Student>().ReverseMap().ForMember(d => d.Name, s => s.MapFrom(std => std.StudentFirstName + " " + std.StudentLastName))
                .ForMember(d => d.timespann, s => s.MapFrom(std => (DateTime.Now - std.EnrollmentDate)));
            CreateMap<StudentRequestDto, Student>().ReverseMap().ForMember(d => d.Name, s => s.MapFrom(std => std.StudentFirstName + " " + std.StudentLastName))
                .ForMember(d => d.timespann, s => s.MapFrom(std => (DateTime.Now - std.EnrollmentDate)))
                .ForMember(n => n.Class, op => op.MapFrom(n => n.Class))
                .AddTransform<String>(n => string.IsNullOrEmpty(n) ? "Class not Found" : n);
            //   .ForMember(d => d.StudentLastName, s => s.Ignore())
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}
