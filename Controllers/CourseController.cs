using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZealEducation.Models;
using ZealEducation.Models.CourseModule;
using ZealEducation.Models.ExamModule;
using ZealEducation.Utils;

namespace ZealEducation.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IWebHostEnvironment _env;

        private string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        public CourseController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllCourses()
        {
            //load all Courses with Sessions descriptions
            var courses = await _dbContext.Course
                .Include(c => c.Sessions)
                .ToListAsync();

            if (courses == null)
            {
                return NotFound();
            }

            return Ok(courses);
        }

        [HttpGet]
        [Route("find/{id}")]
        public async Task<IActionResult> GetCourse([FromRoute] string id,
            [FromQuery] bool returnSessions)
        {
            //load all Courses with Sessions descriptions
            var course = new Course();

            if (returnSessions)
            {
                // Load the course with sessions and resources
                course = await _dbContext.Course
                    .Include(c => c.Sessions)
                    .FirstOrDefaultAsync(c => c.Id == id);

            } else
            {
                course = await _dbContext.Course
                .Include(c => c.Sessions)
                .FirstOrDefaultAsync(c => c.Id == id);
            }

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }


        [HttpGet]
        [Route("find/{id}/session/{sessionNumber}")]
        public async Task<IActionResult> GetSession([FromRoute] string id, int sessionNumber)
        {
            //load Session with session number
            var session = await _dbContext.CourseSession
                .FirstOrDefaultAsync(s => s.Id == IdCreator.ConcatenateId(id, sessionNumber));

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }


        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourseAndSessionsWithResources([FromBody] CourseDTO courseDTO)
        {
            if (courseDTO.Sessions == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                               new Response { Status = "Error", Message = "Please create at least 1 session" });
            }

            //create course
            Course course = new()
            {
                Id = IdCreator.HashId(courseDTO.Name, courseDTO.Description, courseDTO.Price),
                Description = courseDTO.Description,
                Name = courseDTO.Name,
                Price = courseDTO.Price,
                TotalSessions = courseDTO.Sessions.Count,
            };
            _dbContext.Course.Add(course);

            //create Sessions and Resources
            var newSessionsList = courseDTO.Sessions.ToList();
            List<CourseSession> courseSessions = new();
            List<Resource> resources = new();

            for (int i = 0; i < newSessionsList.Count; i++)
            {
                Console.WriteLine("Adding session" + i);
                var newSessionDto = newSessionsList[i];
                CourseSession session = new()
                {
                    Id = IdCreator.ConcatenateId(course.Id, i+1),
                    Course = course,
                    Name = newSessionDto.Name,
                    Description = newSessionDto.Description,
                };

                // Add resources to the session
                if (newSessionDto.Resources != null && newSessionDto.Resources.Any())
                {
                    var newRessourceDtoList = newSessionDto.Resources.ToList();
                    for (int j = 0; j < newRessourceDtoList.Count; j++)
                    {
                        var resourceDto = newRessourceDtoList[j];
                        resources.Add(new Resource
                        {
                            Id = IdCreator.ConcatenateId(session.Id, j+1),
                            Name = resourceDto.Name,
                            FilePath = resourceDto.FilePath,
                            Type = resourceDto.Type,
                            CourseSession = session // Assign the session to the resource
                        });
                    }
                }
                courseSessions.Add(session);
            }
            //Add Sessions to DB
            await _dbContext.CourseSession.AddRangeAsync(courseSessions);
            //Add resources to DB
            await _dbContext.Resource.AddRangeAsync(resources);
            //Save to DB
            await _dbContext.SaveChangesAsync();
            return Created("Course created successfully", course);

        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] string id)
        {
            var course = await _dbContext.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _dbContext.Course.Remove(course);
            await _dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK,
                   new Response { Status = "OK", Message = $"The course with Id {id} have been deleted successfully" });
        }



        [Authorize(Roles = "Admin,Faculty")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCourse(string id, [FromBody] CourseDTO courseDTO)
        {
            var course = await _dbContext.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // Update properties
            course.Name = courseDTO.Name;
            course.Description = courseDTO.Description;
            course.Price = courseDTO.Price;

            _dbContext.Course.Update(course);
            await _dbContext.SaveChangesAsync();

            return Ok($"Course with Id {id} updated successfully"); 
        }

        [HttpPut("update/{id}/image")]
        public async Task<IActionResult> UpdateCourseImage(IFormFile imageFile, [FromRoute] string id)
        {

            // Check file validity
            if (imageFile == null || imageFile.Length <= 0) { return BadRequest("No file submitted"); }

            // Create unique file name 
            string imageFileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            // Check if the file extension is in the list of image extensions
            if (Array.IndexOf(ImageExtensions, imageFileExtension) == -1)
            {
                return BadRequest("The submitted file is not an image");
            }

            var course = await _dbContext.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // Store file in the server
            var folderPath = Path.Combine(_env.ContentRootPath, "Files", "CourseImages");
            Directory.CreateDirectory(folderPath); // Create the folder if it doesn't exist
            var imageName = course.Id + imageFileExtension;
            var imagePath = Path.Combine(folderPath, imageName);


            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            course.ImageName = imageName;

            await _dbContext.SaveChangesAsync();
            return Ok($"Course with Id {id} updated successfully");
        }

        [HttpPost]
        [Route("{id}/session/{number}/resource/create")]
        public async Task<IActionResult> CreateSession([FromRoute] string id, [FromRoute] int number, [FromBody] ResourceDTO resourceDTO)
        {
            var session = await _dbContext.CourseSession.Include(cs => cs.Resources).FirstOrDefaultAsync(cs => cs.Id == IdCreator.ConcatenateId(id, number));
            if (session == null) { return NotFound(); }
            Resource resource = new()
            {
                Id = IdCreator.ConcatenateId(session.Id, session.Resources.Count),
                Name = resourceDTO.Name,
                FilePath = resourceDTO.FilePath,
                Type = resourceDTO.Type,
                CourseSession = session
            };
            return Created("New resource have been added",session);

        }

        [HttpGet]
        [Route("full/all")]
        public async Task<IActionResult> GetAllFullCourses()
        {
            var courses = await _dbContext.Course
                .Include(c => c.Enrollments)
                .Include(c => c.Sessions!).ThenInclude(cs => cs.Resources)
                .ToListAsync();
            if (!courses.Any()) { return NotFound(); }
            return Ok(courses);
        }
    }
}
