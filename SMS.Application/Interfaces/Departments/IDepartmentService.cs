using SMS.Application.Services.Departments.Dto;
using SMS.Common.ViewModels;

namespace SMS.Application.Interfaces.Departments
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetDepartmentListAsync();
        Task<DepartmentDto> GetDepartmentDetailsByIdAsync(Guid DepartmentID);
        Task<ResponseModel> UpdateDepartmentAsync(CreateDepartmentDto DepartmentModel);
        Task<ResponseModel> CreateDepartmentAsync(CreateDepartmentDto DepartmentModel);
        Task<ResponseModel> DeleteDepartmentAsync(Guid DepartmentID);
    }
}
