using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZealEducation.Models.Faculty;

namespace ZealEducation.Controllers
{
    [Authorize(Roles = "Faculty,Admin")]
    [Route("api/[controller]")]
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

        [HttpGet("user")]
        public async Task<IActionResult> Get()
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
    }
}
