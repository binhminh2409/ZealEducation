using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZealEducation.Models;
using ZealEducation.Models.Users;

namespace ZealEducation.Controllers
{
    [Authorize(Roles = "Faculty,Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;


        public AdminController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();

                // Map to UserLists model (optional)
                var userList = new UserList();
                foreach (var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    userList.Users.Add(new UserList.User
                    {
                        Username = u.UserName,
                        Email = u.Email,
                        Roles = roles.ToList()
                    });
                }

                return Ok(userList);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retrieve users");
            }
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
                var userInfo = await _dbContext.UserInfo.ToListAsync();

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retrieve users");
            }
        }


        [HttpGet("enrollment/all")]
        public async Task<IActionResult> GetAllEnrollments ()
        {
            try
            {
                var enrollments = await _dbContext.Enrollment.ToListAsync();

                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retreive enrollments");
            }
        }


        [HttpGet("enrollment/find-by-all-course")]
        public async Task<IActionResult> GetEnrollmentByAllCourse()
        {
            try
            {
                var courses = await _dbContext.Course.Include(c => c.Enrollments).ToListAsync();

                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retreive enrollments");
            }
        }


        [HttpGet("batch")]
        public async Task<IActionResult> GetAllBatches()
        {
            try
            {
                var batches = await _dbContext.Batch
                    .Include(b => b.BatchSessions!).ThenInclude(bs => bs.Attendances)
                    .Include(b => b.Course)
                    .Include(b => b.Exams)
                    .ToListAsync();

                return Ok(batches);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retreive batches");
            }
        }

        [HttpGet("batch-enquiry")]
        public async Task<IActionResult> GetAllBatchEnquiries()
        {
            try
            {
                var batchEnqiries = await _dbContext.BatchEnquiry
                    .ToListAsync();

                return Ok(batchEnqiries);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retreive batche enquiries");
            }
        }


        [HttpGet("user-enquiry")]
        public async Task<IActionResult> GetAllUserEnquiries()
        {
            try
            {
                var userEnqiries = await _dbContext.UserEnquiry
                    .ToListAsync();

                return Ok(userEnqiries);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retreive user enquiries");
            }
        }

        [HttpGet("course-enquiry")]
        public async Task<IActionResult> GetAllCourseEnquiries()
        {
            try
            {
                var courseEnquiries = await _dbContext.CourseEnquiry
                    .ToListAsync();

                return Ok(courseEnquiries);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retreive course enquiries");
            }
        }

    }
}
