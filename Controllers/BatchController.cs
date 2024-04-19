using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ZealEducation.Models;
using ZealEducation.Models.BatchModule;
using ZealEducation.Models.CourseModule;
using ZealEducation.Models.Users;
using ZealEducation.Utils;

namespace ZealEducation.Controllers
{
    [Route("api/batch")]
    [ApiController]
    public class BatchController : ControllerBase
    {   
        private readonly ApplicationDbContext _dbContext;

        //private async Task OnAuthorizationAsync(AuthorizationFilterContext context, string batchId)
        //{
        //    var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //    var userRoleClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        //    if (userIdClaim == null || userRoleClaim == null)
        //    {
        //        context.Result = new StatusCodeResult(501); // User verification error
        //        return;
        //    }

        //    var userId = userIdClaim.Value;
        //    var userRole = userRoleClaim.Value;

        //    // Retrieve the user from the database based on the user ID
        //    var user = await _dbContext.Users.FindAsync(userId);

        //    // Retrieve the course from the database based on the course ID
        //    var batch = await _dbContext.Batch
        //        .Include(b => b.BatchSessions)
        //        .FirstOrDefaultAsync(b => b.Id == batchId);

        //    if (batch == null)
        //    {
        //        context.Result = new NotFoundResult(); // Course not found
        //        return;
        //    }

        //    // Check if the user is enrolled in the course
        //    var enrolled = await _dbContext.Enrollment
        //        .AnyAsync(e => e.User == user && e.Course == batch);
        //    if (!enrolled && userRole == "Candidate")
        //    {
        //        context.Result = new ForbidResult(); // User not enrolled in the course
        //        return;
        //    }
        //}

        public BatchController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllBatches()
        {
            var batches = await _dbContext.Batch
                .Include(b => b.Course)
                .Include(b => b.BatchSessions!).ThenInclude(bs => bs.Attendances!).ThenInclude((Attendance a) => a.UserInfo)
                .ToListAsync();
            if (batches == null) {
                return NotFound();
            }
            
            return Ok(batches);
        }

        [HttpGet]
        [Route("find/{id}")]
        public async Task<IActionResult> FindBatch([FromRoute] string id)
        {
            Batch? batch = new();
            // Find the batch with Sessions included

            batch = await _dbContext.Batch
                    .Include(b => b.Course)
                    .Include(b => b.BatchSessions!).ThenInclude(bs => bs.Attendances!).ThenInclude(a => a.UserInfo)
                    .Include(bs => bs.Exams)
                    .FirstOrDefaultAsync(b => b.Id == id);
            
            if (batch == null)
            {
                return NotFound();
            }

            return Ok(batch);
        }


        [HttpGet]
        [Route("find/{id}/session/{number}")]
        public async Task<IActionResult> GetBatchSession([FromRoute] string id,
            [FromRoute] int number)
        {
            var session = await _dbContext.BatchSession.FirstOrDefaultAsync(b => b.Id == IdCreator.ConcatenateId(id,number));

            if (session == null) { return NotFound(); }
            return Ok(session);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> createBatch([FromBody] BatchDTO batchDTO)
        {
            // Await the FindAsync method to get the actual Course object
            var course = await _dbContext.Course
                .Include(c => c.Sessions)
                .FirstOrDefaultAsync(c => c.Id == batchDTO.CourseId);
            if (course == null) { return BadRequest("Course doesn't exist"); }

            var courseSessions = course.Sessions.ToList();

            // Make sure start and end date are reasonable
            TimeSpan? difference = batchDTO.EndDate - batchDTO.StartDate;
            if (difference.HasValue)
            {
                var days = difference.Value.TotalDays; 
                if ((int)days < course.TotalSessions)
                {
                    return BadRequest(new Response{ Message = "Start and End date must be enough to go through all sessions", Status = "BadRequest" });
                }

            }

            // Create new batch
            Batch batch = new()
            {
                Id = IdCreator.HashId(batchDTO.CourseId, batchDTO.StartDate.ToString(), batchDTO.EndDate.ToString()),
                Course = course,
                StartDate = batchDTO.StartDate,
                EndDate = batchDTO.EndDate,
                Quantity = batchDTO.Quantity,
                TotalSessions = course.TotalSessions,
            };
            // Create batch sesions
            List<BatchSession> batchSessions = new();

            for (int i = 0; i < courseSessions.Count; i++)
            {
                var courseSession = courseSessions[i];
                batchSessions.Add(new BatchSession
                {
                    Id = IdCreator.ConcatenateId(batch.Id, i+1),
                    Batch = batch,
                    Name = courseSession.Name,
                });
            }
            // Save batch and batch sesions to DB
            _dbContext.Add(batch);
            _dbContext.AddRange(batchSessions);

           _dbContext.SaveChanges();

            return Created("Batch created successfuly", batch);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteBatch([FromRoute] string id)
        {
            // Find the batch 
            var batch = await _dbContext.Batch.FindAsync(id);

            if (batch == null)
            {
                return NotFound();
            }

            _dbContext.Remove(batch);
            _dbContext.SaveChanges();
            return Ok(batch);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBatch(string id, [FromBody] BatchDTO batchDTO)
        {
            var batch = await _dbContext.Batch.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }

            // Update properties
            var course = await _dbContext.Course.FindAsync(batchDTO.CourseId);
            if (course == null) 
            {
                return BadRequest("Course doesn't exist");
            }

            batch.Course = course;
            batch.StartDate = batchDTO.StartDate;
            batch.EndDate = batchDTO.EndDate;


            _dbContext.Batch.Update(batch);
            await _dbContext.SaveChangesAsync();

            return Ok(batch);
        }

        [HttpPost]
        [Route("find/{batchId}/assign/candidate/{username}")]
        public async Task<IActionResult> AssignCandidate([FromRoute] string username, [FromRoute] string batchId)
        {
            // Check if user enrolled to the course

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) { return NotFound("Candidate user doesn't exist"); }
            var userInfo = await _dbContext.UserInfo.FirstOrDefaultAsync(ui => ui.User == user);
            // Get the list of all batch sessions
            var batch = await _dbContext.Batch
                .Include(b => b.BatchSessions!).ThenInclude(bs => bs.Attendances!).ThenInclude(a => a.UserInfo)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(b => b.Id == batchId);
            if (batch == null) { return NotFound("Batch doesn't exist"); }
            var batchSessions = batch.BatchSessions!.ToList();
            if (!batchSessions.Any()) { return NotFound("Batch doesn't have any session"); }
            
            // Create a list of attendances
            List<Attendance> attendances = new();
            // Add attendances of the user for each sessions
            foreach ( var session in batchSessions)
            {
                attendances.Add(new Attendance {
                    Id = IdCreator.HashId(session.Id),
                    BatchSession = session,
                    UserInfo = userInfo,
                    Status = "Absent",
                    Date = DateTime.Now,
                });
            }
            // Save to DB
            _dbContext.Attendance.AddRange(attendances);
            _dbContext.SaveChanges();

            return Ok(batch);
        }


        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {

            return Ok();
        }
    }
}
