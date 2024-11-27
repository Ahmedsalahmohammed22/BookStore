using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.AuthorDTOs
{
    public class EditAuthorDTO
    {
        [Required]
        public int ID_Author { get; set; }
        [Required]
        [StringLength(50)]
        public string Author_name { get; set; }
        [StringLength(100)]
        public string Author_Bio { get; set; }
        public int numberOfBooks { get; set; }
        public int Author_age { get; set; }
    }
}
