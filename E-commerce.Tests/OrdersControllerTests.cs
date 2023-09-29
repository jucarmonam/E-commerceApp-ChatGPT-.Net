using E_commerce.Controllers;
using E_commerce.Models;
using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Tests
{
    public class OrdersControllerTests : IDisposable
    {
        private readonly EcommerceContext _context;
        public OrdersControllerTests()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<EcommerceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_OrdersController")
                .Options;

            _context = new EcommerceContext(options);

            // Seed the database with test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 500, Category = new Category { Id = 1, Name = "Category Name" } },
                new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 500, Category = new Category { Id = 2, Name = "Category Name 2" } }
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
        public async Task PostOrder_WithValidOrder_ReturnsCreatedAtAction()
        {
            var controller = new OrdersController(_context);
            var order = new Order 
            { 
                Id = 1,
                UserId = "1", // Assuming you have user authentication
                OrderDate = DateTime.UtcNow,
                TotalAmount = 1000,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Id = 1,
                        Quantity = 1,
                        UnitPrice = 500,
                        ProductId = 1,
                    },
                    new OrderDetail
                    {
                        Id = 2,
                        Quantity = 1,
                        UnitPrice = 500,
                        ProductId = 2,
                    },
                },
            };

            // Act
            var result = await controller.PostOrder(order);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetOrder", createdAtActionResult.ActionName);
            Assert.Equal(order.Id, createdAtActionResult.RouteValues["id"]);
        }
    }
}
