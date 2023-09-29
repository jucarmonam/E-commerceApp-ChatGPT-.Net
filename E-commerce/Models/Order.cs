using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        // Navigation properties for User and OrderDetails relationships
        public string UserId { get; set; }
        public User User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
