using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.CustomerDTOs
{
    public class EditCustomerDTO
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

    }
}
