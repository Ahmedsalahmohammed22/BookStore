using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Catalog
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
  
        public string Description { get; set; }
        
        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
