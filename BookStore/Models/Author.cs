using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Author
    {
        
        public int id { get; set; }
        public string fullname { get; set; }
        [StringLength(100)]
        public string bio { get; set; }
        public int numberOfBooks { get; set; }
        public int age {  get; set; }
        public virtual List<Book> Books { get; set;} = new List<Book>();
    }
}
