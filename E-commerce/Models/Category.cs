namespace E_commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        // Navigation property for the Products relationship
        public List<Product>? Products { get; set; }
    }
}
