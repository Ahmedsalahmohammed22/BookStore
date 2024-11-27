using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
