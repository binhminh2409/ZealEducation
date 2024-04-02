using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZealEducation.Model.Authentication.Signup;
using ZealEducation.Models.Authentication.Login;
using ZealEducation.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZealEducation.Models.Users;

namespace ZealEducation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("signup/candidate")]
        public async Task<IActionResult> RegisterCandidate([FromBody] RegisterUser registerUser)
        {
            //Check User existence
            var userExistByEmail = await _userManager.FindByEmailAsync(registerUser.Email);
            var userExistByUsername = await _userManager.FindByNameAsync(registerUser.Username);
            if (userExistByEmail != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });
            }
            else if (userExistByUsername != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                       new Response { Status = "Error", Message = "Username already taken!" });
            }

            //Add the User in the Database
            User user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                PhoneNumber = registerUser.PhoneNumber,
                DateOfBirth = registerUser.DateOfBirth,
            };

            var role = "Candidate";

            //Add user to the database
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "Candidate user failed to create" });
            }
            //Automatically assign Candidate role
            await _userManager.AddToRoleAsync(user, role);

            return StatusCode(StatusCodes.Status201Created,
                   new Response { Status = "OK", Message = "Candidate user created successfully" });

        }

        [HttpPost]
        [Route("signup/faculty")]
        public async Task<IActionResult> RegisterFaculty([FromBody] RegisterUser registerUser)
        {
            //Check User existence
            var userExistByEmail = await _userManager.FindByEmailAsync(registerUser.Email);
            var userExistByUsername = await _userManager.FindByNameAsync(registerUser.Username);
            if (userExistByEmail != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });
            }
            else if (userExistByUsername != null) {
                return StatusCode(StatusCodes.Status403Forbidden,
                       new Response { Status = "Error", Message = "Username already taken!" });
            }

            //Add the User in the Database
            User user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                PhoneNumber = registerUser.PhoneNumber,
                DateOfBirth = registerUser.DateOfBirth,
            };

            var role = "Faculty";

            //Add user to the database
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "Faculty user failed to create" });
            }
            //Automatically assign Candidate role
            await _userManager.AddToRoleAsync(user, role);
            return StatusCode(StatusCodes.Status201Created,
                   new Response { Status = "OK", Message = "Faculty user created successfully" });

        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            //Check user exists
            var user = await _userManager.FindByNameAsync(loginUser.Username);
            
            //Check password
            if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                //create claimlist 
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                //Add role to the claimlist
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //generate token with the claims
                var jwtToken = GetToken(authClaims);

                //returning the token
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo
                });
            }
            return Unauthorized();

        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}

