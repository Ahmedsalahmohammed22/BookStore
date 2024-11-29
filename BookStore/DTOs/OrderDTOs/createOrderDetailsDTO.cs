using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.OrderDTOs
{
    public class createOrderDetailsDTO
    {
        [Required]
        public int book_id { get; set; }
        [Required]
        public int quantity { get; set;}

    }
}
