using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Teachers;
using SMS.Application.Services.Teachers.Dto;

namespace SMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // POST: api/teacher
        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto teacherModel)
        {
            var response = await _teacherService.CreateTeacherAsync(teacherModel);
            if (!response.Successful)
            {
                return BadRequest(response.Message);
            }

            return CreatedAtAction(nameof(GetTeacherDetailsById), new { id = teacherModel.Id }, response);
        }

        // GET: api/teacher
        [HttpGet]
        public async Task<IActionResult> GetTeacherList()
        {
            var teachers = await _teacherService.GetTeacherListAsync();
            return Ok(teachers);
        }

        // GET: api/teacher/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherDetailsById(Guid id)
        {
            var teacher = await _teacherService.GetTeacherDetailsByIdAsync(id);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            return Ok(teacher);
        }

        // PUT: api/teacher
        [HttpPut]
        public async Task<IActionResult> UpdateTeacher([FromBody] CreateTeacherDto teacherModel)
        {
            var response = await _teacherService.UpdateTeacherAsync(teacherModel);
            if (!response.Successful)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        // DELETE: api/teacher/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var response = await _teacherService.DeleteTeacherAsync(id);
            if (!response.Successful)
            {
                return NotFound("Teacher not found.");
            }

            return NoContent();
        }

        // GET: api/teacher/cnic/{cnic}
        [HttpGet("cnic/{cnic}")]
        public async Task<IActionResult> GetTeacherDetailsByCnic(string cnic)
        {
            var teachers = await _teacherService.GetTeacherDetailsByCNICAsync(cnic);
            if (teachers == null || teachers.Result == null || !teachers.Result.Any())
            {
                return NotFound("No teachers found with the provided CNIC.");
            }

            return Ok(teachers);
        }
    }
}