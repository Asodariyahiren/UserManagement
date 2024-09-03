using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User_Management_API.Models;
using User_Management_API.Models.Authentication.SignUp;

namespace User_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser, string role)
        {
            //Check User exist or not
            var userExist = await _userManager.FindByEmailAsync(registerUser.EmailAddress);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new APIResponse { Status = "Error", Message = "User Already Exist." });
            }
            //Add User in database
            IdentityUser user = new()
            {
                Email = registerUser.EmailAddress,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName
            };

            //Check role is exist or not
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new APIResponse { Status = "Error", Message = "User Failed to Create" });
                }
                //add role to the user 
                await _userManager.AddToRoleAsync(user, role);
                return StatusCode(StatusCodes.Status200OK,
                        new APIResponse { Status = "Success", Message = "User Created Successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new APIResponse { Status = "Errpr", Message = "This Role doesn't Exist." });
            }
        }
    }
}
