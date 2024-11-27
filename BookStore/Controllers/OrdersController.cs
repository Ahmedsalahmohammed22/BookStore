using BookStore.DTOs.AuthorDTOs;
using BookStore.DTOs.OrderDTOs;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class OrdersController : ControllerBase
    {
        BookStoreContext _context;
        UserManager<IdentityUser> _userManager;

        public OrdersController(BookStoreContext context , UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            Customer customer = (Customer) await _userManager.FindByNameAsync(User.Identity.Name);
            if (createOrderDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            List<OrderDetails> ordersDetails = new List<OrderDetails>();
            decimal TotalPrice = 0;
            Order order = new Order()
            {
                cust_id = customer.Id,
                status = createOrderDto.status,
                Orderdetails = ordersDetails,
                totalprice = TotalPrice
            };
            foreach (var orderDetails in createOrderDto.OrderDetails)
            {
                OrderDetails ordDetails = new OrderDetails()
                {
                    book_id = orderDetails.book_id,
                    unitprice = orderDetails.unitPrice,
                    quantity = orderDetails.quantity,
                    order = order
                };
                TotalPrice += ordDetails.unitprice * ordDetails.quantity;
                ordersDetails.Add(ordDetails);
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            List<OrderDetailDto> ordersDtoDetail = new List<OrderDetailDto>();
            foreach (var orderDetailsDTO in order.Orderdetails)
            {
                OrderDetailDto ordDtoDetails = new OrderDetailDto()
                {
                    BookId = orderDetailsDTO.book_id,
                    UnitPrice = orderDetailsDTO.unitprice,
                    Quantity = orderDetailsDTO.quantity,
                };
                ordersDtoDetail.Add(ordDtoDetails);
            }
            OrderDTO orderDTO = new OrderDTO()
            {
                Id = order.id,
                OrderDate = order.orderDate,
                TotalPrice = order.totalprice,
                Status = order.status,
                OrderDetails = ordersDtoDetail
            };

            return Ok(orderDTO);
        }
        [Authorize (Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            List<Order> orders = await _context.Orders.ToListAsync();
            if (!orders.Any()) return NotFound();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach (var order in orders)
            {
                List<OrderDetailDto> ordersDtoDetail = new List<OrderDetailDto>();

                foreach (var orderDetailsDTO in order.Orderdetails)
                {
                    OrderDetailDto ordDtoDetails = new OrderDetailDto()
                    {
                        BookId = orderDetailsDTO.book_id,
                        UnitPrice = orderDetailsDTO.unitprice,
                        Quantity = orderDetailsDTO.quantity,
                    };
                    ordersDtoDetail.Add(ordDtoDetails);
                }
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = order.id,
                    OrderDate = order.orderDate,
                    TotalPrice = order.totalprice,
                    Status = order.status,
                    OrderDetails = ordersDtoDetail
                };
                orderDTOs.Add(orderDTO);
            }
            return Ok(orderDTOs.ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            Order order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            List<OrderDetailDto> ordersDtoDetail = new List<OrderDetailDto>();
            foreach (var orderDetailsDTO in order.Orderdetails)
            {
                OrderDetailDto ordDtoDetails = new OrderDetailDto()
                {
                    BookId = orderDetailsDTO.book_id,
                    UnitPrice = orderDetailsDTO.unitprice,
                    Quantity = orderDetailsDTO.quantity,
                };
                ordersDtoDetail.Add(ordDtoDetails);
            }
            OrderDTO orderDTO = new OrderDTO()
            {
                Id = order.id,
                OrderDate = order.orderDate,
                TotalPrice = order.totalprice,
                Status = order.status,
                OrderDetails = ordersDtoDetail
            };
            return Ok(orderDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateStatus(int id,[FromBody] string status)
        {
            if (id <= 0) return BadRequest("Invalid Order ID");
            Order order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound($"Order with ID {id} not found.");
            order.status = status;  
            if (await _context.SaveChangesAsync() > 0) return NoContent();
            else return BadRequest();
        }
    }
}
