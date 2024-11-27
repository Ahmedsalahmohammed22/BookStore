using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        public int id { get; set; }
        [Required]
        [StringLength(150)]
        public string title { get; set; }
        [Column(TypeName="money")]
        public decimal price { get; set; }
        public int stock {  get; set; }
        public string photoPath { get; set; }
        [Column(TypeName = "date")]

        public DateOnly publishDate { get; set; }
        [ForeignKey("author")]
        public int? author_id { get; set; }
        public virtual Author author { get; set; } 
        [ForeignKey("catalog")]
        public int? catalog_id { get; set; }
        public virtual Catalog catalog { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();


    }
}
