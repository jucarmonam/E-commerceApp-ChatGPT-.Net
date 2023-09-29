using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        // Navigation property for the Category relationship
        public int CategoryId { get; set; }
        public required Category Category { get; set; }

        // Navigation property for OrderDetails relationship (many-to-many)
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
