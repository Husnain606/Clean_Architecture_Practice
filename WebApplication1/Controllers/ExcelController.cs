using Microsoft.AspNetCore.Mvc;
using SMS.Application.Interfaces.Excel;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcelFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            var entities = await _excelService.ReadExcelFileAsync(file,1,2,true);
            return Ok(entities); // Return the saved data for verification
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetExcelData()
        {
            var entities = await _excelService.GetAllExcelDataAsync(1,2);
            return Ok(entities); // This will return the data to be viewed in Swagger
        }

        [HttpPost("getAll")]
        public async Task<IActionResult> GetallData(IFormFile file)
        {
            
       
            var entities = await _excelService.ReadExcelFileAsync(file,1,2,false);
            if (entities == null || file.Length == 0)
                return BadRequest("No file provided.");

            return Ok(entities); // Return the saved data for verification
        }
    }
}
