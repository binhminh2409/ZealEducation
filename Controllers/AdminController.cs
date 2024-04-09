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
                var userInfo = _dbContext.UserInfo.ToList();

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                // Handle any errors 
                return StatusCode(500, "Failed to retrieve users");
            }
        }
    }
}
