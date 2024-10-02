using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SMS.Application.Interfaces;
using SMS.Application.Interfaces.Excel;
using SMS.Application.Services.Students.Dto;
using SMS.Common.ViewModels;
using SMS.Domain.Entities;
using System;
using System.Drawing.Printing;

namespace SMS.Application.Services.Excel
{
    public class ExcelService : IExcelService
    {
        private readonly IApplicationDbContext _context;

        public ExcelService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> ReadExcelFileAsync(IFormFile file, int pageNumber, int pageSize, bool isupload)
        {
            try
            {
                ResponseModel model = new ResponseModel();
                List<Department> entities = new List<Department>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                await FetchingDataFromSql(file, entities);

                if (isupload == true) await SaveExcelDataAsync(entities);
                int totalRecords = entities.Count();

                // No need to await LINQ operations on in-memory collections.
                var pagedDepartments = entities
                    .OrderBy(d => d.DepartmentName) // Apply sorting (optional)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();  // Convert the result to a list.

                var pagedResult = new PagedResult<Department>
                {
                    Items = pagedDepartments,  // Use the paginated result.
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                model.data = pagedResult;
                model.IsSuccess = true;
                model.StatusCode = System.Net.HttpStatusCode.OK;
                return model;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                throw new Exception("Error while reading Excel file", ex);
            }
        }

        private static async Task FetchingDataFromSql(IFormFile file, List<Department> entities)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var entity = new Department
                        {
                            Id = Guid.NewGuid(),
                            DepartmentName = worksheet.Cells[row, 2].Value?.ToString(),
                            DepartmenrDescription = worksheet.Cells[row, 3].Value?.ToString(),
                            // Map other properties based on your Excel columns
                        };
                        entities.Add(entity);
                    }
                }
            }
        }

        public async Task SaveExcelDataAsync(List<Department> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    var existingEntity = await _context.Department.FindAsync(entity.Id);
                    if (existingEntity == null)
                    {
                        // Entity is not tracked, add it
                        await _context.Department.AddAsync(entity);
                    }
                    else
                    {
                        // Optionally, update the existing entity or skip
                        // _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                throw new Exception("Error while saving Excel data to the database", ex);
            }
        }

        public async Task<ResponseModel> GetAllExcelDataAsync(int pageNumber, int pageSize)
        {
            try
            {
                ResponseModel model = new ResponseModel();
                int totalRecords = await _context.Department.CountAsync();
              var deparment=  await _context.Department
                    .OrderBy(d => d.DepartmentName) // Apply sorting (optional)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();    // Fetch the paginated records from the database
                var pagedResult = new PagedResult<Department>
                {
                    Items = deparment,
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                model.data = pagedResult;
                model.IsSuccess = true;
                model.StatusCode = System.Net.HttpStatusCode.OK;
                 return model;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                throw new Exception("Error while fetching all Excel data", ex);
            }
        }
    }
}
