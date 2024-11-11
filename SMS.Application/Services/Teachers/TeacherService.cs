using AutoMapper;
using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Teachers;
using SMS.Application.Interfaces;
using SMS.Application.Services.Teachers.Dto;
using SMS.Common.ViewModels;
using SMS.Common.Constants;
using SMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SMS.Common.Utilities;
using SMS.Application.Services.Account.Dto;
using SMS.Application.Interfaces.Identity;

namespace SMS.Application.Services.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TeacherService> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;


        public TeacherService(IIdentityService identityService,IRepository<Teacher> teacherRepository, IMapper mapper, ILogger<TeacherService> logger, IApplicationDbContext dbContext)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
            _identityService = identityService;
        }

        // CREATE TEACHER
        public async Task<ResponseModel> CreateTeacherAsync(CreateTeacherDto teacherModel)
        {
            ResponseModel<TeacherDto> model = new ResponseModel<TeacherDto>();

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create a new ApplicationUser 
                var applicationUser = new ApplicationUser
                {
                    UserName = teacherModel.FirstName+teacherModel.LastName, // Assuming the email is used as the username
                    Email = teacherModel.Mail,
                    CreatedAt = DateTime.Now,
                };

                // Create the user in the Resultbase
                CreateUserDto user = new CreateUserDto();
                var result = await _identityService.CreateUserAsync(applicationUser, teacherModel.Pasword);
                if (!result.Succeeded)
                {
                    model.Successful = false;
                    model.Message = string.Join("; ", result.Errors.Select(e => e.Description));
                    return model;
                }

                // Map the student model to the Student entity
                var teacher = _mapper.Map<Teacher>(teacherModel);
                teacher.Id = Guid.NewGuid();
                teacher.UserId = applicationUser.Id; // Associate the student with the created user

                // Create the student in the Resultbase
                var Result = await _teacherRepository.CreateAsync(teacher);
                await transaction.CommitAsync();

                model.Result = Result;
                model.Successful = true;
                model.Message = StudentConstants.successMessage;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating teacher.");
                model.Successful = false;
                model.Message = "An error occurred while creating the teacher.";
            }

            return model;
        }

        // GET THE LIST OF ALL TEACHERS
        public async Task<ResponseModel<List<TeacherDto>>> GetTeacherListAsync()
        {
            ResponseModel<List<TeacherDto>> responseModel = new ResponseModel<List<TeacherDto>>();
            try
            {
                _logger.LogInformation("Getting all the teachers executed.");
                var teachers = await _teacherRepository.GetAllAsync();
                responseModel.Result = _mapper.Map<List<TeacherDto>>(teachers);
                responseModel.Successful = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting teacher list.");
                responseModel.Successful = false;
                responseModel.Message = "An error occurred while retrieving the teacher list.";
            }
            return responseModel;
        }

        // GET TEACHER DETAILS BY TEACHER ID
        public async Task<ResponseModel<TeacherDto>> GetTeacherDetailsByIdAsync(Guid teacherId)
        {
            ResponseModel<TeacherDto> model = new ResponseModel<TeacherDto>();
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    model.Successful = false;
                    model.Message = TeacherConstants.notFound;
                    _logger.LogInformation(TeacherConstants.notFound);
                    return model;
                }
                var teacherDTO = _mapper.Map<TeacherDto>(teacher);
                teacherDTO.timespann = CheckTime.GetTimeDifference(teacher.HiringDate); // Assuming CheckTime is a utility for time difference
                model.Result = teacherDTO;
                model.Successful = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting teacher details.");
                model.Successful = false;
                model.Message = "An error occurred while retrieving teacher details.";
            }
            return model;
        }

        // UPDATE TEACHER
        public async Task<ResponseModel> UpdateTeacherAsync(CreateTeacherDto teacherModel)
        {
            ResponseModel model = new ResponseModel();
            
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(teacherModel.Id);
                if (teacher == null)
                {
                    model.Successful = false;
                    model.Message = TeacherConstants.notFound;
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
                model.Successful = false;
                model.Message = "An error occurred while updating the teacher.";
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
                model.Successful = false;
                model.Message = "An error occurred while deleting the teacher.";
            }
            return model;
        }

        // GET TEACHER DETAILS BY CNIC
        public async Task<ResponseModel<List<TeacherDto>>> GetTeacherDetailsByCNICAsync(string cnic)
        {
            ResponseModel<List<TeacherDto>> responseModel = new ResponseModel<List<TeacherDto>>();
            try
            {
                _logger.LogInformation("Getting teacher details by CNIC: {CNIC}", cnic);
                var teachers = await _teacherRepository.Table.Where(t => t.CNIC == cnic).Distinct().ToListAsync();
                if (teachers == null || !teachers.Any())
                {
                    _logger.LogInformation(TeacherConstants.notFound);
                    responseModel.Successful = false;
                    responseModel.Message = TeacherConstants.notFound;
                    return responseModel;
                }
                responseModel.Result = _mapper.Map<List<TeacherDto>>(teachers);
                responseModel.Successful = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting teacher details by CNIC.");
                responseModel.Successful = false;
                responseModel.Message = "An error occurred while retrieving teacher details by CNIC.";
            }
            return responseModel;
        }

       
    }
}