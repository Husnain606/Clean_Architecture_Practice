using Microsoft.AspNetCore.Http;
using SMS.Common.ViewModels;
using SMS.Domain.Entities;


namespace SMS.Application.Interfaces.Excel
{
    public interface IExcelService
    {
        Task<ResponseModel> ReadExcelFileAsync(IFormFile file, int pageNumber, int pageSize , bool isupload);
        Task SaveExcelDataAsync(List<Department> entities);
        Task<ResponseModel> GetAllExcelDataAsync(int pageNumber, int pageSize);
    }
}
