using BookStore.Models;

namespace BookStore.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
