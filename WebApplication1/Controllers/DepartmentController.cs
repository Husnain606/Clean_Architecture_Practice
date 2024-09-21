using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Departments;
using SMS.Application.Services.Departments.Dto;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Student")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentServices;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(IMapper mapper, ILogger<DepartmentController> logger, IDepartmentService _departmentServices)
        {
            _mapper = mapper;
            _logger = logger;
            departmentServices = _departmentServices;
        }

        // GET ALL Department
        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await departmentServices.GetDepartmentListAsync();
                if (departments == null) return NotFound();
                return Ok(departments);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET Department DETAILS BY ID
        [HttpGet("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            try
            {
                var department = await departmentServices.GetDepartmentDetailsByIdAsync(id);
                if (department == null)
                {
                    _logger.LogInformation("Department Not Found with ID = {0}!!", id);
                    return NotFound();
                }
                return Ok(department);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // CREATE Department
        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto departmentModel)
        {
            try
            {
                var model = await departmentServices.CreateDepartmentAsync(departmentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // UPDATE Department
        [HttpPut("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(CreateDepartmentDto departmentModel)
        {
            try
            {
                var model = await departmentServices.UpdateDepartmentAsync(departmentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE Department
        [HttpDelete("DeleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                var model = await departmentServices.DeleteDepartmentAsync(id);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
