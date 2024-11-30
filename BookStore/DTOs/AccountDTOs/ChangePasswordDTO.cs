using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.AccountDTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string oldPassword { get; set; }
        [Required]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\\W)(?!.* ).{8,16}$")]
        public string newPassword { get; set; }
        [Required]
        [Compare("newPassword", ErrorMessage = "Passwords do not match.")]
        public string confirmPassword { get; set; }
    }
}
