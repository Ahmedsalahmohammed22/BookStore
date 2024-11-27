using BookStore.DTOs.AdminDTO;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        BookStoreContext _context;
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        public AdminController(BookStoreContext context , UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = (await _userManager.GetUsersInRoleAsync("admin")).OfType<Admin>().ToList();
            if(!admins.Any()) return NotFound();
            return Ok(admins);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminDTO adminDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Admin admin = new Admin()
            {
                UserName = adminDTO.Name,
                Email = adminDTO.Email,
                PhoneNumber = adminDTO.PhoneNumber,
            };
            IdentityResult r = await _userManager.CreateAsync(admin,adminDTO.Password); 
            if (r.Succeeded) 
            {
                IdentityResult adminRole = await _userManager.AddToRoleAsync(admin , "admin");
                if (adminRole.Succeeded)
                {
                    return Ok();
                }
                else return BadRequest(adminRole.Errors);
            }else return BadRequest(r.Errors);
        }
    }
}
