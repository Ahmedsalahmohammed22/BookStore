using BookStore.DTOs.AccountDTOs;
using BookStore.DTOs.CustomerDTOs;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        BookStoreContext _context;
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<IdentityUser> _signInManager;

        public AccountsController(BookStoreContext context, RoleManager<IdentityRole> roleManager , UserManager<IdentityUser> userManager , SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;


        }
        [HttpPost("Register")]
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
                var r = await _userManager.CreateAsync(customer, registerDTO.password);
                if (!r.Succeeded) return BadRequest(r.Errors);
                var roleResult = await _userManager.AddToRoleAsync(customer, "customer");
                if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);
                   
                return Ok(registerDTO);
            }
            else  return BadRequest(ModelState); 
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _userManager.FindByNameAsync(loginDTO.username);
            if (user == null) return Unauthorized("Invalid user");
            var result = await _userManager.CheckPasswordAsync(user , loginDTO.password);
            if(!result) return Unauthorized("Invalid password");
            var roles = await _userManager.GetRolesAsync(user);
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
        public async Task<IActionResult> changePassword(ChangePasswordDTO pass)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(pass.id);
                if (user == null) return NotFound();
                var r = await _userManager.ChangePasswordAsync(user, pass.oldPassword, pass.newPassword);
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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new
            {
                message = "You have been successfully logged out.",
                timestamp = DateTime.UtcNow
            });
        }

    }
}
