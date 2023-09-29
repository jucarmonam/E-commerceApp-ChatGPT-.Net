using E_commerce.Controllers;
using E_commerce.Models;
using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Tests
{
    public class ProductsControllerTests : IDisposable
    {
        private readonly EcommerceContext _context;

        public ProductsControllerTests()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<EcommerceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_ProductsController")
                .Options;

            _context = new EcommerceContext(options);

            // Seed the database with test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Description = "Description 1", Category = new Category { Id = 1, Name = "Category Name" } },
                new Product { Id = 2, Name = "Product 2", Description = "Description 2", Category = new Category { Id = 2, Name = "Category Name 2" } }
            };

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            // Clean up the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithProducts()
        {
            // Arrange
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, products.Count);
        }
    }
}
