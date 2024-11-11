using AutoMapper;
using SMS.Application.Services.Students.Dto;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<CreateStudentDto, Student>()
                .ReverseMap();

            CreateMap<StudentDto, Student>().ReverseMap()
                .ForMember(d => d.Name, s => s.MapFrom(std => std.FirstName + " " + std.LastName));

            CreateMap<StudentRequestDto, Student>().ReverseMap()
                .ForMember(d => d.Name, s => s.MapFrom(std => std.FirstName + " " + std.LastName))
                .ForMember(d => d.timespann, s => s.MapFrom(std => (DateTime.Now - std.EnrollmentDate)))
                .ForMember(n => n.Class, op => op.MapFrom(n => n.Class))
                .AddTransform<string>(n => string.IsNullOrEmpty(n) ? "Class not Found" : n);
                

            // If there are any other mappings required, add them here.
        }
    }
}
