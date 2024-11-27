using BookStore.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.AuthorDTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BIO { get; set; }
        public int NumberOfBooks { get; set; }
        public int Age { get; set; }
        public List<String>? books { get; set; }
    }
}
