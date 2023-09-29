using E_commerce.Controllers;
using E_commerce.Models;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Security.Claims;

namespace E_commerce.Pages
{
    public class SuccessModel : PageModel
    {
        private readonly OrdersController _ordersController;

        public SuccessModel(OrdersController ordersController)
        {
            _ordersController = ordersController;
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                var cartItems = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(HttpContext.Session.GetString("CartItems"));
                var order = new Order
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Assuming you have user authentication
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = CalculateTotalPrice(cartItems),
                    OrderDetails = new List<OrderDetail>(),
                    // Add other order details like shipping address, payment method, etc.
                };

                foreach (var product in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = product.ProductId,
                        Quantity = product.Quantity, // Set the quantity or any other relevant details
                        UnitPrice = product.Price
                    };

                    order.OrderDetails.Add(orderDetail);
                }

                HttpContext.Session.Clear();

                await _ordersController.PostOrder(order);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, data not available)
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Page();
        }

        public decimal CalculateTotalPrice(List<ShoppingCartItem> cartItems)
        {
            return cartItems.Sum(item => item.Price * item.Quantity);
        }
    }
}
