using E_commerce.Controllers;
using E_commerce.Models;
using E_commerce.Services;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace E_commerce.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProductsController _productsController;

        public IndexModel(ILogger<IndexModel> logger, ProductsController productsController)
        {
            _logger = logger;
            _productsController = productsController;
        }

        public List<Product> Products { get; set; }
        private List<ShoppingCartItem> cartItems = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Call a method from your controller
                IActionResult actionResult = await _productsController.GetProducts();

                // Check if the result is an OkObjectResult (assuming your controller returns Ok() for success)
                if (actionResult is OkObjectResult okObjectResult)
                {
                    Products = okObjectResult.Value as List<Product>;

                    // Process the returned data
                }
                else
                {
                    // Handle other types of results (e.g., NotFound, BadRequest, etc.)
                    // You can set an error message or perform other actions as needed.
                    return NotFound(); // Or return an appropriate IActionResult
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network errors, data not available)
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return Page();
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            // Retrieve the list of products again
            OnGetAsync().Wait();
            var product = Products.First(product => product.Id == productId);
            if (product != null)
            {
                // Storing data in the session
                AddToCart(product);
                //_shoppingCartService.AddToCart(product);
                HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(cartItems));
            }

            return RedirectToPage("/Cart"); // Redirect to the cart page or another suitable page
        }

        public void AddToCart(Product product)
        {
            // Check if the product is already in the cart
            if(HttpContext.Session.GetString("CartItems") != null)
            {
                cartItems = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(HttpContext.Session.GetString("CartItems"));
            }

            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                };
                cartItems.Add(newItem);
            }
        }
    }
}