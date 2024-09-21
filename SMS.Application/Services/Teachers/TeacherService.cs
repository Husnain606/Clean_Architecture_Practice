using AutoMapper;
using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Teachers;
using SMS.Application.Interfaces;
using SMS.Application.Services.Teachers.Dto;
using SMS.Common.ViewModels;
using SMS.Common.Constants;
using SMS.Common.Utilities;
using SMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SMS.Application.Services.Teachers.Validators;

namespace SMS.Application.Services.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TeacherService> _logger;
        private readonly IApplicationDbContext _dbContext;

        public TeacherService(IRepository<Teacher> teacherRepository, IMapper mapper, ILogger<TeacherService> logger, IApplicationDbContext dbContext)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }

        // CREATE TEACHER
        public async Task<ResponseModel> CreateTeacherAsync(CreateTeacherDto teacherModel)
        {
            ResponseModel model = new ResponseModel();

            // Validate the teacher model
            var _validator = new TeacherValidator();
            var validationResult = await _validator.ValidateAsync(teacherModel);
            if (!validationResult.IsValid)
            {
                model.IsSuccess = false;
                model.Messsage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return model;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var teacher = _mapper.Map<Teacher>(teacherModel);
                teacher.Id = Guid.NewGuid();
                model = await _teacherRepository.CreateAsync(teacher);
                await transaction.CommitAsync();

                model.IsSuccess = true;
                model.Messsage = TeacherConstants.successMessage;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating teacher.");
                throw ex;
            }

            return model;
        }

        // GET THE LIST OF ALL TEACHERS
        public async Task<List<TeacherDto>> GetTeacherListAsync()
        {
            try
            {
                _logger.LogInformation("Getting all the teachers executed !!");
                var teachers = await _teacherRepository.GetAllAsync();
                if (teachers == null) return null;
                var teacherDTOs = _mapper.Map<List<TeacherDto>>(teachers);

                return teacherDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting teacher list.");
                throw;
            }
        }

        // GET TEACHER DETAILS BY TEACHER ID
        public async Task<TeacherDto> GetTeacherDetailsByIdAsync(Guid teacherId)
        {
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    _logger.LogInformation(TeacherConstants.notFound);
                    return null;
                }
                var teacherDTO = _mapper.Map<TeacherDto>(teacher);
                teacherDTO.timespann= CheckTime.GetTimeDifference(teacher.HiringDate);
                return teacherDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting teacher details.");
                throw;
            }
        }

        // UPDATE TEACHER
        public async Task<ResponseModel> UpdateTeacherAsync(CreateTeacherDto teacherModel)
        {
            ResponseModel model = new ResponseModel();
            var _validator = new TeacherValidator();
            var validationResult = await _validator.ValidateAsync(teacherModel);
            if (!validationResult.IsValid)
            {
                model.IsSuccess = false;
                model.Messsage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return model;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(teacherModel.Id);
                if (teacher == null)
                {
                    model.IsSuccess = false;
                    model.Messsage = TeacherConstants.notFound;
                    _logger.LogInformation(TeacherConstants.notFound, teacherModel.Id);
                    return model;
                }

                var updatedTeacher = _mapper.Map<Teacher>(teacherModel);
                model = await _teacherRepository.UpdateAsync(updatedTeacher);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating teacher.");
                throw ex;
            }
            return model;
        }

        // DELETE TEACHER
        public async Task<ResponseModel> DeleteTeacherAsync(Guid teacherId)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                model = await _teacherRepository.DeleteAsync(teacherId);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while deleting teacher.");
                throw;
            }
            return model;
        }

        // GET TEACHER DETAILS BY CNIC
        public async Task<List<TeacherDto>> GetTeacherDetailsByCnicAsync(String cnic)
        {
            try
            {
                var teachers = await _teacherRepository.Table.Where(t => t.CNIC == cnic).Distinct().ToListAsync();
                if (teachers == null) return null;

                var teacherDTOs = _mapper.Map<List<TeacherDto>>(teachers);
                return teacherDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting teacher by CNIC.");
                throw;
            }
        }
        public async Task<List<TeacherDto>> GetTeacherDetailsByCNICAsync(string cnic)
        {
            try
            {
                // Logging the action
                _logger.LogInformation("Getting teacher details by CNIC: {CNIC}", cnic);

                // Query the teacher repository based on the provided CNIC
                var teachers = await _teacherRepository.Table
                    .Where(t => t.CNIC == cnic)
                    .ToListAsync();

                // If no teachers found, return null or an empty list based on your requirement
                if (teachers == null || !teachers.Any())
                {
                    _logger.LogInformation(TeacherConstants.notFound);
                    return null;  // You can return an empty list instead: return new List<TeacherDto>();
                }

                // Map the result to TeacherDto
                var teacherDTOs = _mapper.Map<List<TeacherDto>>(teachers);

                return teacherDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting teacher details by CNIC.");
                throw;
            }
        }

    }
}
