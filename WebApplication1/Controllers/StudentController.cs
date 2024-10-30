using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Students;
using SMS.Application.Services.Students.Dto;
using Microsoft.AspNetCore.Authorization;
using SMS.Application.Services.Common;
using SMS.Common.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StudentController : ControllerBase
    {

        private readonly IStudentService studentServices;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentController> _logger;   // 	D – Dependency Inversion Principle (DIP):
                                                               // 	Injecting Logger rather then inheriting through inheritince
                                                               // 	a high level module or main module is restricted to implement an low level module 
        public StudentController(IMapper mapper, ILogger<StudentController> logger, IStudentService _studentServices)
        {
            _mapper = mapper;
            _logger = logger;
            studentServices = _studentServices;
        }
        [AllowAnonymous]
        // GET ALL STUDENT
        [HttpGet("GetAll")]
        public async Task<ActionResult<ResponseModel<GridDto<StudentDto>>>> GetAllStudents([FromQuery] StudentRequestDto request)
        {
            try
            {
                var students = await studentServices.GetStudentListAsync(request);
                if (students == null) return NotFound();
                return Ok(students);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET STUDENT DETAILS BY ID
        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            try
            {
                var student = await studentServices.GetStudentDetailsByIdAsync(id);
                if (student == null)
                {
                    _logger.LogInformation("Student Not Found with ID = {0}!!", id);
                    return NotFound();
                }

                return Ok(student);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET STUDENT DETAILS BY  JOINS 
        [HttpGet("GetStudentByAge\"")]
        public async Task<IActionResult> GetStudentByAge()
        {
            try
            {
                var student = await studentServices.GetStudentDetailsByAgeG13Async(13);
                if (student == null)
                {
                    _logger.LogInformation("Student Not Found with Age = {0}!!");
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception)
            {
                throw;
            }
        }


        // CREATE STUDENT
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent(CreateStudentDto studentModel)
        {
            try
            {
                var model = await studentServices.CreateStudentAsync(studentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // UPDATE STUDENT
        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(CreateStudentDto studentModel)
        {
            try
            {
                var model = await studentServices.UpdateStudentAsync(studentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE STUDENT
        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var model = await studentServices.DeleteStudentAsync(id);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
