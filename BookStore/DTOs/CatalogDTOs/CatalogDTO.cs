using BookStore.DTOs.BookDTOs;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.CatalogDTOs
{
    public class CatalogDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public List<BookDetailsDTOs> Books { get; set; }
    }
}
