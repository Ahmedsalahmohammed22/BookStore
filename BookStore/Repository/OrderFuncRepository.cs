using BookStore.DTOs.BookDTOs;
using BookStore.DTOs.OrderDTOs;
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class OrderFuncRepository
    {
        BookStoreContext _context;
        public OrderFuncRepository(BookStoreContext context)
        {
            _context = context;
        }
        public List<OrderDTO> convertOrdersToOrderDTO(List<Order> orders)
        {
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
            return orderDTOs.ToList();
        }
        public OrderDTO convertOrderToOrderDTO(Order order)
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
            return orderDTO;
        }
        public async Task<List<OrderDTO>> GetCustomerOrders(string id)
        {

            List<Order> orders = await _context.Orders.Where(o => o.cust_id == id).ToListAsync();
            return convertOrdersToOrderDTO(orders).ToList();
        }

    }
}
