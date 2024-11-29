using BookStore.DTOs.AdminDTO;
using BookStore.DTOs.BookDTOs;
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        UnitOfWork _unit;
        public AdminController(UnitOfWork unit)
        {
            _unit = unit;
        }
        [HttpGet]
        [SwaggerOperation(Summary = "Get all admins",
                  Description = "Fetches all users with the 'admin' role from the system.")]
        [SwaggerResponse(200, "List of admins retrieved successfully.", typeof(List<Admin>))]
        [SwaggerResponse(404, "No admins found.")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = (await _unit.UserReps.GetUsersWithRole("admin")).OfType<Admin>().ToList();
            if(!admins.Any()) return NotFound();
            return Ok(admins);
        }
        [HttpPost("CreateAdmin")]
        [SwaggerOperation(Summary = "Create a new admin",
                  Description = "Creates a new user with the 'admin' role.")]
        [SwaggerResponse(200, "Admin created successfully.")]
        [SwaggerResponse(400, "Invalid input or failed creation.")]
        public async Task<IActionResult> CreateAdmin(AdminDTO adminDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Admin admin = new Admin()
            {
                UserName = adminDTO.Name,
                Email = adminDTO.Email,
                PhoneNumber = adminDTO.PhoneNumber,
            };
            IdentityResult r = await _unit.UserReps.CreateUser(admin,adminDTO.Password); 
            if (r.Succeeded) 
            {
                IdentityResult adminRole = await _unit.UserReps.AddRole(admin , "admin");
                if (adminRole.Succeeded)
                {
                    return Ok();
                }
                else return BadRequest(adminRole.Errors);
            }else return BadRequest(r.Errors);
        }
    }
}
