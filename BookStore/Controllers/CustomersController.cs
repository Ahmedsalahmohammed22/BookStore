using BookStore.DTOs.AccountDTOs;
using BookStore.DTOs.CustomerDTOs;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "admin")]
    public class CustomersController : ControllerBase
    {
        BookStoreContext _context;
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        public CustomersController(BookStoreContext context , UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize (Roles = "customer")]
        [HttpPut]
        public async Task<IActionResult> editProfile(EditCustomerDTO _customer)
        {
            if (ModelState.IsValid)
            {
                Customer customer = (Customer)await _userManager.FindByNameAsync(User.Identity.Name);
                if (customer == null) return NotFound();
                customer.UserName = _customer.name;
                customer.Email = _customer.email;
                customer.address = _customer.address;
                customer.PhoneNumber = _customer.phone;
                customer.fullname = _customer.username;
                var r = await _userManager.UpdateAsync(customer);
                if (r.Succeeded) return NoContent();
                else return BadRequest(r.Errors);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
             var users = (await _userManager.GetUsersInRoleAsync("customer")).OfType<Customer>().ToList();
            if (!users.Any()) return NotFound();
            List<SelectCustomerDTO> cstDTO = new List<SelectCustomerDTO>();
            foreach (var user in users)
            {
                SelectCustomerDTO cDTO = new SelectCustomerDTO()
                {
                    Id = user.Id,
                    fullname = user.fullname,
                    address = user.address,
                    username = user.UserName,
                    email = user.Email,
                    phonenumber = user.PhoneNumber
                };
                cstDTO.Add(cDTO);
            }
            return Ok(cstDTO);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getbyid(string id)
        {
            Customer customer = (Customer) _userManager.GetUsersInRoleAsync("customer").Result.Where(c => c.Id == id).FirstOrDefault();
            if (customer == null) return NotFound();
            SelectCustomerDTO selectCustomer = new SelectCustomerDTO()
            {
                Id = customer.Id,
                fullname = customer.fullname,
                address = customer.address,
                email = customer.Email,
                username = customer.UserName,
                phonenumber = customer.PhoneNumber
            };
            return Ok(selectCustomer);

        }
    }
}
