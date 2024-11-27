using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.BookDTOs
{
    public class AddBookDTO
    {
        [Required]
        [StringLength(150)]
        public string title { get; set; }
        [Required]
        public decimal price { get; set; }
        public int stock { get; set; }
        public IFormFile? photoPath { get; set; }
        public DateOnly publishDate { get; set; }
        public int author_id { get; set; }
        public int catalog_id { get; set; }

    }
}
