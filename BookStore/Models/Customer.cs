using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class Customer:IdentityUser
    {
        public string fullname { get; set; }
        public string address { get; set; }
        public int age { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
