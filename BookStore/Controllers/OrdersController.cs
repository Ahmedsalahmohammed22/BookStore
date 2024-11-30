using BookStore.DTOs.AuthorDTOs;
using BookStore.DTOs.OrderDTOs;
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        UnitOfWork _unit;

        public OrdersController(UnitOfWork unit)
        {
            _unit = unit;
        }
        [Authorize(Roles = "customer")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new order",
                  Description = "Creates a new order for the authenticated customer with book details and quantities.")]
        [SwaggerResponse(200, "Order created successfully", typeof(OrderDTO))]
        [SwaggerResponse(400, "Invalid request data or order quantity issues")]
        [SwaggerResponse(404, "Book not found or insufficient stock")]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            Customer customer = (Customer) await _unit.UserReps.GetUserByName(User.Identity.Name);
            if (createOrderDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //List<OrderDetails> ordersDetails = new List<OrderDetails>();
            decimal TotalPrice = 0;
            Order order = new Order()
            {
                cust_id = customer.Id,
                status = "create",
                orderDate = DateTime.Now,
            };
            foreach (var orderDetails in createOrderDto.OrderDetails)
            {
                Book book = await _unit.BookReps.Get(orderDetails.book_id);
                TotalPrice += book.price * orderDetails.quantity;
                OrderDetails ordDetails = new OrderDetails()
                {
                    order_id = order.id,
                    book_id = orderDetails.book_id,
                    unitprice = book.price,
                    quantity = orderDetails.quantity,
                };
                if(book.stock > ordDetails.quantity)
                {
                    order.Orderdetails.Add(ordDetails);
                    book.stock -= ordDetails.quantity;


                }else return BadRequest("invalid quantity");

            }
            order.totalprice = TotalPrice;
            await _unit.Save();

            _unit.OrderReps.Add(order);
            await _unit.Save();
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
        [SwaggerOperation(Summary = "Get all orders",
                  Description = "Retrieves a list of all orders in the system. Only accessible by admin.")]
        [SwaggerResponse(200, "List of orders retrieved successfully", typeof(List<OrderDTO>))]
        [SwaggerResponse(404, "No orders found")]
        public async Task<IActionResult> GetOrders()
        {
            List<Order> orders = await _unit.OrderReps.GetAll();
            if (!orders.Any()) return NotFound();
            return Ok(_unit.OrderFuncRepository.convertOrdersToOrderDTO(orders));
        }
        [Authorize (Roles = "admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get order by ID",
                  Description = "Retrieves the details of a specific order by its ID.")]
        [SwaggerResponse(200, "Order details retrieved successfully", typeof(OrderDTO))]
        [SwaggerResponse(404, "Order not found")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            Order order = await _unit.OrderReps.Get(id);
            if (order == null) return NotFound();
            return Ok(_unit.OrderFuncRepository.convertOrderToOrderDTO(order));
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update order status",
                  Description = "Updates the status of a specific order.")]
        [SwaggerResponse(204, "Order status updated successfully")]
        [SwaggerResponse(400, "Invalid status or order ID")]
        [SwaggerResponse(404, "Order not found")]
        public async Task<IActionResult> updateStatus(int id,[FromBody] string status)
        {
            if (id <= 0) return BadRequest("Invalid Order ID");
            Order order = await _unit.OrderReps.Get(id);
            if (order == null) return NotFound($"Order with ID {id} not found.");
            order.status = status;  
            if (await _unit.Save() > 0) return NoContent();
            else return BadRequest();
        }
        [Authorize(Roles = "customer")]
        [HttpGet("customer/orders")]
        [SwaggerOperation(Summary = "Get customer orders",
                  Description = "Retrieves a list of orders placed by the authenticated customer.")]
        [SwaggerResponse(200, "Customer orders retrieved successfully", typeof(List<OrderDTO>))]
        [SwaggerResponse(404, "No orders found for the customer")]
        public async Task<IActionResult> GetCustomerOrders()
        {
            Customer customer = (Customer) await _unit.UserReps.GetUserByName(User.Identity.Name);
            if (customer == null) return NotFound();
            var orderCust = await _unit.OrderFuncRepository.GetCustomerOrders(customer.Id);
            if(orderCust == null) return NotFound();   
            return Ok(orderCust);
        }
    }
}
