using SMS.Application.Services.Teachers.Dto;
using SMS.Common.ViewModels;

namespace SMS.Application.Interfaces.Teachers
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetTeacherListAsync();
        Task<TeacherDto> GetTeacherDetailsByIdAsync(Guid teacherId);
        Task<ResponseModel> CreateTeacherAsync(CreateTeacherDto teacherModel);
        Task<ResponseModel> UpdateTeacherAsync(CreateTeacherDto teacherModel);
        Task<ResponseModel> DeleteTeacherAsync(Guid teacherId);
        Task<List<TeacherDto>> GetTeacherDetailsByCNICAsync(string cnic);
    }
}