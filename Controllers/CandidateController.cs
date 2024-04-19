using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZealEducation.Models;
using ZealEducation.Models.CandidateModule;
using ZealEducation.Utils;

namespace ZealEducation.Controllers
{
    [Authorize]
    [Route("api/candidate")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public CandidateController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpPost]
        [Route("create/enrollment")]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentDTO enrollmentDTO)
        {
            // Check if the course exist
            var course = await _dbContext.Course.FindAsync(enrollmentDTO.CourseId);
            if (course == null) { return BadRequest("Course not exist"); }

            // Compare the amount: 
            if (enrollmentDTO.Amount != course.Price) { return BadRequest("Can not enroll: Amount doesn't match course's price"); }

            // Find user info: 
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var userInfo = await _dbContext.UserInfo.FindAsync(userIdClaim.Value);

            // Create enrollment
            Enrollment enrollment = new()
            {
                Id = IdCreator.HashId(enrollmentDTO.CourseId, enrollmentDTO.Note),
                Amount = enrollmentDTO.Amount,
                Course = course,
                CreatedDate = DateTime.UtcNow,
                Note = enrollmentDTO.Note,
                Status = "Success",
                UserInfo = userInfo
            };

            await _dbContext.Enrollment.AddAsync(enrollment);
            await _dbContext.SaveChangesAsync();
            return Created("Enrolled successfully", userInfo);
        }

        [HttpGet]
        [Route("find/enrollment/all")]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var userInfo = await _dbContext.UserInfo.FindAsync(userIdClaim.Value);

            var enrollments = await _dbContext.Enrollment
                .Where(e => e.UserInfo == userInfo)
                .ToListAsync();
            return Ok(enrollments);
        }

        [HttpGet]
        [Route("find/batch/all")]
        public async Task<IActionResult> GetAllBatches()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var userInfo = await _dbContext.UserInfo.FindAsync(userIdClaim.Value);

            // Find all attendances
            var batches = await _dbContext.Batch
                .Include(b => b.Exams!).ThenInclude(e => e.Submissions!).ThenInclude(sub => sub.UserInfo).Where(b => b.Exams!.Any(e => e.Submissions!.Any(sub => sub.UserInfo == userInfo)))
                .Include(b => b.Course)
                .Include(b => b.BatchSessions!).ThenInclude(bs => bs.Attendances).Where(b => b.BatchSessions!.Any(bs => bs.Attendances!.Any(a => a.UserInfo == userInfo)))
                .ToListAsync();
            if (batches == null) { return NotFound(); }

            return Ok(batches);
        }

        [HttpPut]
        [Route("update/attendance/{sessionId}")] 
        public async Task<IActionResult> Attend([FromRoute] string sessionId)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var userInfo = await _dbContext.UserInfo.FindAsync(userIdClaim.Value);

            // Find the Attendance
            var batchSession = await _dbContext.BatchSession.FindAsync(sessionId);
            if (batchSession == null) { return NotFound(); }

            // Update attendance
            var attendance = await _dbContext.Attendance.FirstOrDefaultAsync(a => a.UserInfo == userInfo && a.BatchSession == batchSession);
            if (attendance == null) { return NotFound("Not assigned to this session"); }
            attendance.Status = "Present";
            attendance.Date = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return Ok(attendance);
            
        }

        [HttpGet]
        [Route("learn/{batchId}")]
        public async Task<IActionResult> GetAllLearningResouces([FromRoute] string batchId)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var userInfo = await _dbContext.UserInfo.FindAsync(userIdClaim.Value);

            // Find batch
            var batch = await _dbContext.Batch
                .Include(b => b.Course).ThenInclude(c => c.Sessions!).ThenInclude(cs => cs.Resources)
                .Include(b => b.BatchSessions!).ThenInclude(bs => bs.Attendances)
                .FirstOrDefaultAsync(b => b.Id == batchId 
                    && b.BatchSessions!.Any(bs => bs.Attendances!.Any(a => a.UserInfo == userInfo)));


            if (batch == null) { return NotFound(); }

            // Check if batch time is still valid for learning
            if (batch.StartDate > DateTime.UtcNow) { return Unauthorized( new Response() { Message = $"This batch hasn't started yet. Please comeback at {batch.StartDate}", Status = "401" }); }
            else if (batch.EndDate < DateTime.UtcNow) { return Unauthorized(new Response() { Message = $"This batch has been closed since {batch.EndDate}", Status = "401" }); }

            // return the course with it's resources
            if (batch.Course == null) { return NotFound("Course not found"); }
            var course = batch.Course;

            return Ok(course);
        }

    }
}
