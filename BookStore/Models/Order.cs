using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Order
    {
        public int id { get; set; }
        [Column(TypeName = "date")]

        public DateTime orderDate { get; set; } = DateTime.Now; 
        [Column(TypeName = "money")]
        public decimal totalprice { get; set; }
        public string status { get; set; }
        [ForeignKey("Customer")]
        public string cust_id { get; set; }
        public virtual Customer customer { get; set; }
        public virtual List<OrderDetails> Orderdetails { get; set; } = new List<OrderDetails>();


    }
}
