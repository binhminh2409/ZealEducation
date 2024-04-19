using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ZealEducation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public TestController(IWebHostEnvironment env)
        {
            _env = env;
        }


        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var folderPath = Path.Combine(_env.ContentRootPath, "Files");
                Directory.CreateDirectory(folderPath); // Create the folder if it doesn't exist

                var filePath = Path.Combine(folderPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok($"File uploaded successfully. File path: {filePath}");
            }
            else
            {
                return BadRequest("No file uploaded or file is empty.");
            }
        }
    }
}

