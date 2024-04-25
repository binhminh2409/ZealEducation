using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZealEducation.Model.Authentication.Signup;
using ZealEducation.Models;
using ZealEducation.Models.BatchModule;
using ZealEducation.Models.CandidateModule;
using ZealEducation.Models.Users;
using ZealEducation.Utils;

namespace ZealEducation.Controllers
{
    [Authorize]
    [Route("api/candidate")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly UserManager<User> _userManager;

        public CandidateController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                // Find user info: 
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null) { return Unauthorized(); }
                var userInfo = await _dbContext.UserInfo
                    .Include(u => u.Enrollments)
                    .Include(u => u.Attendances)
                    .Include(u => u.Submissions)
                    .FirstOrDefaultAsync(u => u.Id == userIdClaim.Value);
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retrieve user info");
            }
        }

        [HttpPut("update/user-info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoDTO userInfoDTO)
        {
            try
            {
                // Find user info: 
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null) { return Unauthorized(); }
                var userInfo = await _dbContext.UserInfo
                    .Include(u => u.Enrollments)
                    .Include(u => u.Attendances)
                    .Include(u => u.Submissions)
                    .FirstOrDefaultAsync(u => u.Id == userIdClaim.Value);
                var user = await _userManager.FindByIdAsync(userIdClaim.Value);
                // Check if email existed
                var userExistByEmail = await _userManager.FindByEmailAsync(userInfoDTO.Email);
                if (userExistByEmail != null)
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new Response { Status = "Error", Message = "Email already exists!" });
                }

                userInfo.Email = userInfoDTO.Email;
                userInfo.FirstName = userInfoDTO.FirstName;
                userInfo.LastName = userInfoDTO.LastName;
                userInfo.PhoneNumber = userInfoDTO.PhoneNumber;
                userInfo.DateOfBirth = userInfoDTO.DateOfBirth;
                user.Email = userInfoDTO.Email;

                await _userManager.UpdateAsync(user);
                _dbContext.Update(userInfo);
                await _dbContext.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retrieve user info");
            }
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
        [Route("find/course/all")]
        public async Task<IActionResult> GetAllCourses()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var userInfo = await _dbContext.UserInfo.FindAsync(userIdClaim.Value);

            var courses = await _dbContext.Course
                .Where(c => c.Enrollments.Any(e => e.UserInfo == userInfo))
                .ToListAsync();
            return Ok(courses);
        }

        [HttpGet]
        [Route("find/batch/all")]
        public async Task<IActionResult> GetAllBatches()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var userInfo = await _dbContext.UserInfo.FindAsync(userIdClaim.Value);

            var batches = await _dbContext.Batch
                .Include(b => b.Course)
                .Include(b => b.BatchSessions!).ThenInclude(bs => bs.Attendances!).ThenInclude((Attendance a) => a.UserInfo).Where(b => b.BatchSessions!.Any(bs => bs.Attendances!.Any(a => a.UserInfo == userInfo)))
                .Include(b => b.Exams)
                .ToListAsync();
            if (batches == null)
            {
                return NotFound();
            }


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
