using AutoMapper;
using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Departments;
using SMS.Application.Interfaces;
using SMS.Application.Services.Departments.Dto;
using SMS.Common.ViewModels;
using SMS.Domain.Entities;
using SMS.Application.Services.Departments.Validators;

namespace SMS.Application.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IApplicationDbContext _dbContext;

        public DepartmentService(IRepository<Department> departmentRepository, IMapper mapper, ILogger<DepartmentService> logger, IApplicationDbContext dbContext)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }
        // CREATE DEPARTMENT
        public async Task<ResponseModel> CreateDepartmentAsync(CreateDepartmentDto DepartmentModel)
        {
            ResponseModel model = new ResponseModel();
            var _validator = new DepartmentValidator();
            var validationResult = await _validator.ValidateAsync(DepartmentModel);
            if (!validationResult.IsValid)
            {
                model.Successful = false;
                model.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return model;
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var department = _mapper.Map<Department>(DepartmentModel);
                department.Id = Guid.NewGuid();
                model = await _departmentRepository.CreateAsync(department);
                await transaction.CommitAsync();

                model.Successful = true;
                model.Message = "Department Created Successfully";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating department.");
                throw ex;
            }
            return model;
        }

        // GET THE LIST OF ALL Departments
        public async Task<List<DepartmentDto>> GetDepartmentListAsync()
        {
            try
            {
                _logger.LogInformation("Getting all the departments executed !!");
                var departments = await _departmentRepository.GetAllAsync();
                if (departments == null) return null;
                var departmentDTOs = _mapper.Map<List<DepartmentDto>>(departments);
                return departmentDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting department list.");
                throw;
            }
        }

        // GET Department DETAILS BY Department ID
        public async Task<DepartmentDto> GetDepartmentDetailsByIdAsync(Guid DepartmentID)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(DepartmentID);
                if (department == null)
                {
                    _logger.LogInformation("Department Not Found with ID = {0}!!", DepartmentID);
                    return null;
                }
                var departmentDTO = _mapper.Map<DepartmentDto>(department);
                return departmentDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting department details.");
                throw;
            }
        }

        // UPDATE Department
        public async Task<ResponseModel> UpdateDepartmentAsync(CreateDepartmentDto DepartmentModel)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var department = await _departmentRepository.GetByIdAsync(DepartmentModel.Id);
                if (department == null)
                {
                    model.Successful = false;
                    model.Message = $"Department Not Found with ID = {DepartmentModel.Id}!!";
                    _logger.LogInformation("Department Not Found with ID = {0}!!", DepartmentModel.Id);
                    return model;
                }
                var updatedDepartment = _mapper.Map<Department>(DepartmentModel);
                model = await _departmentRepository.UpdateAsync(updatedDepartment);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating department.");
                throw ex;
            }
            return model;
        }

        // DELETE Department
        public async Task<ResponseModel> DeleteDepartmentAsync(Guid DepartmentID)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                model = await _departmentRepository.DeleteAsync(DepartmentID);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while deleting department.");
                model.Successful = false;
                model.Message = "Error occurred while deleting department.";
            }
            return model;
        }
    }
}
