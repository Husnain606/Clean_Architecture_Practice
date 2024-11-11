using AutoMapper;
using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Students;
using SMS.Application.Interfaces;
using SMS.Application.Services.Students.Dto;
using SMS.Common.ViewModels;
using SMS.Common.Constants;
using SMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SMS.Application.Services.Common;
using System.Linq.Dynamic.Core;
using SMS.Common.Extensions;
using AutoMapper.QueryableExtensions;
using SMS.Application.Services.Account.Dto;
using SMS.Application.Interfaces.Identity;
using FluentValidation;

namespace SMS.Application.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        private readonly IApplicationDbContext _dbContext; 
        private readonly IUserRoleService _userRole;

        public StudentService(IRepository<Student> studentRepository,
            IMapper mapper,
            ILogger<StudentService> logger,
            IApplicationDbContext dbContext,
            IIdentityService identityService,
            IUserRoleService userRole)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
            _identityService = identityService;
            _userRole = userRole;
        }

        // CREATE STUDENT
        public async Task<ResponseModel> CreateStudentAsync(CreateStudentDto studentModel)
        {
            ResponseModel<StudentDto> model = new ResponseModel<StudentDto>();

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create a new ApplicationUser 
                var applicationUser = new ApplicationUser
                {
                    UserName = studentModel.FirstName+studentModel.LastName, // Assuming the email is used as the username
                    Email = studentModel.Mail,
                    CreatedAt = DateTime.Now,
                  
                };
                // Create the user in the Resultbase
                CreateUserDto user = new CreateUserDto();
                var result = await _identityService.CreateUserAsync(applicationUser, studentModel.Pasword);
                if (!result.Succeeded)
                {
                    model.Successful = false;
                    model.Message = string.Join("; ", result.Errors.Select(e => e.Description));
                    return model;
                }
                await _userRole.AssignRoleAsync(applicationUser.Id, "9595c536-8516-4d9a-872b-6f336e4e3716");

                // Map the student model to the Student entity
                var student = _mapper.Map<Student>(studentModel);
                student.Id = Guid.NewGuid();
                student.UserId = applicationUser.Id; // Associate the student with the created user

                // Create the student in the Resultbase
                var Result = await _studentRepository.CreateAsync(student);
                model.Result = Result;
                model.Successful = true;
                model.Message = StudentConstants.successMessage;
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating student.");
                model.Successful = false;
                model.Message = "An error occurred while creating the student.";
            }

            return model;
        }

        // GET THE LIST OF ALL STUDENTS WITH PAGINATION
        public async Task<ResponseModel<GridDto<StudentDto>>> GetStudentListAsync(StudentRequestDto request)
        {
            try
            {
                var paging = new GridDto<StudentDto>(); // Changed to StudentDto
                _logger.LogInformation($"{nameof(GetStudentListAsync)} {ApplicationLogsConstants.MethodRunning}");
                IQueryable<Student> students = GetStudentQuery(request);
                _logger.LogInformation("Getting all the students executed with pagination !!");

                paging.TotalRecords = await students.CountAsync();
                paging.Data = await students.PageBy(request.PageNumber, request.PageSize)
                    .ProjectTo<StudentDto>(_mapper.ConfigurationProvider).ToListAsync();


                _logger.LogInformation($"{nameof(StudentDto)} {ApplicationLogsConstants.MethodExecuted}");

                // Return the result with StudentDto paging
                return new ResponseModel<GridDto<StudentDto>> { Successful = true, Result =paging };
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
                  .OrderByIf(string.IsNullOrEmpty(request.SortBy), x => x.FirstName);
        }


        // GET STUDENT DETAILS BY STUDENT ID
        public async Task<ResponseModel> GetStudentDetailsByIdAsync(Guid studentId)
        {
            try
            {
                ResponseModel<StudentDto> model = new ResponseModel<StudentDto>();
                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null)
                {
                    _logger.LogInformation(StudentConstants.notFound);
                    return null;
                }
                var studentDTO = _mapper.Map<StudentRequestDto>(student);
                //studentDTO.timespann = CheckTime.GetTimeDifference(student.EnrollmentDate);

                model.Result = studentDTO;
                model.Successful = true;
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
            
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var student = await _studentRepository.GetByIdAsync(studentModel.Id);
                if (student == null)
                {
                    model.Successful = false;
                    model.Message = StudentConstants.notFound;
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