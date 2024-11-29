using BookStore.DTOs.AccountDTOs;
using BookStore.DTOs.CustomerDTOs;
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
    [Authorize (Roles = "admin")]
    public class CustomersController : ControllerBase
    {
        UnitOfWork _unit;
        public CustomersController(UnitOfWork unit)
        {
            _unit = unit;
        }
        [Authorize (Roles = "customer")]
        [HttpPut]
        [SwaggerOperation(Summary = "Edit user profile details",
                  Description = "Allows the authenticated user to update their profile details.")]
        [SwaggerResponse(204, "Profile updated successfully")]
        [SwaggerResponse(400, "Bad request due to invalid data")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> editProfile(EditCustomerDTO _customer)
        {
            if (ModelState.IsValid)
            {
                Customer customer = (Customer)await _unit.UserReps.GetUserByName(User.Identity.Name);
                if (customer == null) return NotFound();
                customer.UserName = _customer.name;
                customer.Email = _customer.email;
                customer.address = _customer.address;
                customer.PhoneNumber = _customer.phone;
                customer.fullname = _customer.username;
                var r = await _unit.UserReps.UpdateUser(customer);
                if (r.Succeeded) return NoContent();
                else return BadRequest(r.Errors);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all customers",
                  Description = "Retrieves a list of all customers along with their profile information.")]
        [SwaggerResponse(200, "List of customers retrieved successfully", typeof(List<SelectCustomerDTO>))]
        [SwaggerResponse(404, "No customers found")]
        public async Task<IActionResult> GetAllAsync()
        {
             var users = (await _unit.UserReps.GetUsersWithRole("customer")).OfType<Customer>().ToList();
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
        [SwaggerOperation(Summary = "Get customer by ID",
                  Description = "Retrieves the profile details of a customer by their unique ID.")]
        [SwaggerResponse(200, "Customer details retrieved successfully", typeof(SelectCustomerDTO))]
        [SwaggerResponse(404, "Customer not found")]
        public async Task<IActionResult> getbyid(string id)
        {
            Customer customer = (Customer) _unit.UserReps.GetUsersWithRole("customer").Result.Where(c => c.Id == id).FirstOrDefault();
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
