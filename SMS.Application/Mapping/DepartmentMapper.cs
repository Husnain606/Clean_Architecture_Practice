using AutoMapper;
using SMS.Application.Services.Departments.Dto;
using SMS.Domain.Entities;

namespace SMS.Application.Mapping
{
    public class DepartmentMapper : Profile
    {
        public DepartmentMapper()
        {
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
            CreateMap<DepartmentDto, Department>().ReverseMap();
        }
    }
}
