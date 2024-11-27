namespace BookStore.DTOs.OrderDTOs
{
    public class OrderDetailDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
