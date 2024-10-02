using AutoMapper;
using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Students;
using SMS.Application.Interfaces;
using SMS.Application.Services.Students.Dto;
using SMS.Common.ViewModels;
using SMS.Common.Constants;
using SMS.Common.Utilities;
using SMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SMS.Application.Services.Students.Validators;
using SMS.Application.Services.Common;
using System.Linq.Dynamic.Core;
using SMS.Common.Extensions;
using AutoMapper.QueryableExtensions;

namespace SMS.Application.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        private readonly IApplicationDbContext _dbContext;
       
        public StudentService(IRepository<Student> studentRepository, IMapper mapper, ILogger<StudentService> logger, IApplicationDbContext dbContext)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }

        // CREATE STUDENT
        public async Task<ResponseModel> CreateStudentAsync(CreateStudentDto studentModel)
        {
            ResponseModel model = new ResponseModel();

            // Validate the student model
            var _validator = new StudentValidator();
            var validationResult = await _validator.ValidateAsync(studentModel);
            if (!validationResult.IsValid)
            {
                model.IsSuccess = false;
                model.Messsage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return model;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var student = _mapper.Map<Student>(studentModel);
                student.Id = Guid.NewGuid();
                model = await _studentRepository.CreateAsync(student);
                await transaction.CommitAsync();

                model.IsSuccess = true;
                model.Messsage = StudentConstants.successMessage;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating student.");
                throw ex;
            }

            return model;
        }

        // GET THE LIST OF ALL STUDENTS WITH PAGINATION
        public async Task<ResponseModel<GridDto<StudentDto>>> GetStudentListAsync(StudentRequestDto request)
        {
            try
            {
                ResponseModel responsemodel = new ResponseModel();
                var paging = new GridDto<StudentDto>(); // Changed to StudentDto
                _logger.LogInformation($"{nameof(GetStudentListAsync)} {ApplicationLogsConstants.MethodRunning}");
                IQueryable<Student> students = GetStudentQuery(request);
                _logger.LogInformation("Getting all the students executed with pagination !!");

                paging.TotalRecords = await students.CountAsync();
                paging.Data = await students.PageBy(request.PageNumber, request.PageSize)
                    .ProjectTo<StudentDto>(_mapper.ConfigurationProvider).ToListAsync();


                _logger.LogInformation($"{nameof(StudentDto)} {ApplicationLogsConstants.MethodExecuted}");

                // Return the result with StudentDto paging
                return new ResponseModel<GridDto<StudentDto>> { IsSuccess = true, Result = paging };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting student list.");
                throw;
            }
        }

        private IQueryable<Student> GetStudentQuery(StudentRequestDto request)
        {
            return _studentRepository.TableNoTracking.TagWith("GetStudentQuery")
                  .OrderByIf(!string.IsNullOrEmpty(request.SortBy), request.SortBy!, request.SortOrder)
                  .OrderByIf(string.IsNullOrEmpty(request.SortBy), x => x.StudentFirstName);
        }


        // GET STUDENT DETAILS BY STUDENT ID
        public async Task<ResponseModel> GetStudentDetailsByIdAsync(Guid studentId)
        {
            try
            {
                ResponseModel model = new ResponseModel();
                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null)
                {
                    _logger.LogInformation(StudentConstants.notFound);
                    return null;
                }
                var studentDTO = _mapper.Map<StudentRequestDto>(student);
                //studentDTO.timespann = CheckTime.GetTimeDifference(student.EnrollmentDate);

                model.data = studentDTO;
                model.IsSuccess = true;
                model.StatusCode = System.Net.HttpStatusCode.OK;
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting student details.");
                throw;
            }
        }

        // UPDATE STUDENT
        public async Task<ResponseModel> UpdateStudentAsync(CreateStudentDto studentModel)
        {
            ResponseModel model = new ResponseModel();
            var _validator = new StudentValidator();
            var validationResult = await _validator.ValidateAsync(studentModel);
            if (!validationResult.IsValid)
            {
                model.IsSuccess = false;
                model.Messsage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return model;
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var student = await _studentRepository.GetByIdAsync(studentModel.Id);
                if (student == null)
                {
                    model.IsSuccess = false;
                    model.Messsage = StudentConstants.notFound;
                    _logger.LogInformation(StudentConstants.notFound, studentModel.Id);
                    return model;
                }
                var updatedStudent = _mapper.Map<Student>(studentModel);
                model = await _studentRepository.UpdateAsync(updatedStudent);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating student.");
                throw ex;
            }
            return model;
        }

        // DELETE STUDENT
        public async Task<ResponseModel> DeleteStudentAsync(Guid studentId)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                model = await _studentRepository.DeleteAsync(studentId);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while deleting student.");
                throw;
            }
            return model;
        }
        // GET DETAIL BY SPECIFICATION
        public async Task<List<StudentRequestDto>> GetStudentDetailsByAgeG13Async(int age)
        {
            try
            {
                var students = await _studentRepository.Table.Where(s => s.Age == age).Distinct().ToListAsync();
                if (students == null) return null;

                var studentDTOs = _mapper.Map<List<StudentRequestDto>>(students);
                return studentDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting students by age.");
                throw;
            }
        }



    }
}
