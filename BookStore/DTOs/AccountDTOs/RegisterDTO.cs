using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.AccountDTOs
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(100)]
        public string name { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string email { get; set; }
        [RegularExpression("^01[0125][0-9]{8}$")]
        public string phone { get; set; }
        [StringLength(100)]
        public string address { get; set; }
        [Required]
        [StringLength(100)]
        public string username { get; set; }
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\\W)(?!.* ).{8,16}$")]   
        public string password { get; set; }
        [Compare("password", ErrorMessage = "Passwords do not match.")]
        public string confirmPassword { get; set; }
        [Range(15, 100)]
        public int age { get; set; }

    }
}
