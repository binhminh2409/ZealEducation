using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public AdminController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();

                // Map to UserLists model (optional)
                var userList = new UserLists();
                foreach (var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    userList.Users.Add(new UserLists.User
                    {
                        Username = u.UserName,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        PhoneNumber = u.PhoneNumber,
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

        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("Candidate");

                // Map to UserLists model (optional)
                var userList = new UserLists();
                foreach (var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    userList.Users.Add(new UserLists.User
                    {
                        Username = u.UserName,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        PhoneNumber = u.PhoneNumber,
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
    }
}
