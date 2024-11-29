using BookStore.DTOs.AccountDTOs;
using BookStore.DTOs.CustomerDTOs;
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        UnitOfWork _unit;

        public AccountsController(UnitOfWork unit)
        {
            _unit = unit;
        }
        [HttpPost("Register")]
        [SwaggerOperation(Summary = "Register a new customer account",
                  Description = "Registers a new customer with provided details and assigns them the 'customer' role.")]
        [SwaggerResponse(200, "Registration successful", typeof(RegisterDTO))]
        [SwaggerResponse(400, "Invalid input or registration failed")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(ModelState.IsValid)
            {
                if (registerDTO.password != registerDTO.confirmPassword) return BadRequest("confirm password not equal password");
                
                Customer customer = new Customer()
                {
                    UserName = registerDTO.name,
                    fullname = registerDTO.username,
                    PhoneNumber = registerDTO.phone,
                    address = registerDTO.address,
                    age = registerDTO.age,
                    Email = registerDTO.email,
                };
                var r = await _unit.UserReps.CreateUser(customer, registerDTO.password);
                if (!r.Succeeded) return BadRequest(r.Errors);
                var roleResult = await _unit.UserReps.AddRole(customer, "customer");
                if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);
                   
                return Ok(registerDTO);
            }
            else  return BadRequest(ModelState); 
        }
        [HttpPost("Login")]
        [SwaggerOperation(Summary = "User Login",
                  Description = "Authenticates the user and returns a JWT token if the credentials are valid.")]
        [SwaggerResponse(200, "Login successful. Returns a JWT token and user data.", typeof(CustomerDTO))]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(401, "Invalid username or password.")]

        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _unit.UserReps.GetUserByName(loginDTO.username);
            if (user == null) return Unauthorized("Invalid user");
            var result = await _unit.UserReps.CheckPassword(user , loginDTO.password);
            if(!result) return Unauthorized("Invalid password");
            var roles = await _unit.UserReps.GetRoles(user);
            List<Claim> userdata = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),

            };
            userdata.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            string key = "Book store api secret key for token validation";
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            var signingcer = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims:userdata,
                expires:DateTime.Now.AddDays(1),
                signingCredentials: signingcer
                ) ;
            var finalToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new CustomerDTO
            {
                username = loginDTO.username,
                token = finalToken,
                email = user.Email
            });

        }
        [Authorize]
        [HttpPost("changePassword")]
        [SwaggerOperation(Summary = "Change user password",
                  Description = "Allows a user to change their password by providing the old and new passwords.")]
        [SwaggerResponse(200, "Password changed successfully.")]
        [SwaggerResponse(404, "User not found.")]
        [SwaggerResponse(400, "Invalid input or password change failed.")]
        public async Task<IActionResult> changePassword(ChangePasswordDTO pass)
        {

            if (ModelState.IsValid)
            {
                var user = await _unit.UserReps.GetUserById(pass.id);
                if (user == null) return NotFound();
                var r = await _unit.UserReps.ChangePassword(user, pass.oldPassword, pass.newPassword);
                if (r.Succeeded) return Ok();
                else return BadRequest(r.Errors);
            }
            else
            {
                return BadRequest(ModelState);
            }


        }
        [Authorize]
        [HttpGet("Logout")]
        [SwaggerOperation(Summary = "Logout the current user",
                  Description = "Ends the session for the authenticated user by clearing their authentication data.")]
        [SwaggerResponse(200, "Logout successful.", typeof(object))]
        public async Task<IActionResult> Logout()
        {
            await _unit.UserReps.Logout();
            return Ok(new
            {
                message = "You have been successfully logged out.",
                timestamp = DateTime.UtcNow
            });
        }

    }
}
