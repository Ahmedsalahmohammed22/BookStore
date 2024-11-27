using BookStore.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.OrderDTOs
{
    public class CreateOrderDto
    {

        [Required]
        public string status { get; set; }
        [Required]
        public List<createOrderDetailsDTO> OrderDetails { get; set; }

    }
}
