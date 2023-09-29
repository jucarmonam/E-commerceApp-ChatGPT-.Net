using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Drawing2D;

namespace E_commerce.Pages
{
    public class CartModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<ShoppingCartItem> CartItems { get; set; }
        public decimal TotalPrice { get; set; }

        public CartModel(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            CartItems = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(HttpContext.Session.GetString("CartItems"));
            TotalPrice = CalculateTotalPrice();
            return Page();
        }

        public IActionResult OnGetRemoveFromCart(int id)
        {
            // Retrieve cart items from session storage and deserialize them
            var cartItemsJson = HttpContext.Session.GetString("CartItems");
            CartItems = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(cartItemsJson);

            var itemToRemove = CartItems.FirstOrDefault(item => item.ProductId == id);
            if (itemToRemove != null)
            {
                CartItems.Remove(itemToRemove);
                HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(CartItems));
            }
            return RedirectToPage();
        }

        public decimal CalculateTotalPrice()
        {
            return CartItems.Sum(item => item.Price * item.Quantity);
        }

        public IActionResult OnPostCheckoutStripe()
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];
            // Retrieve cart items from session storage and deserialize them
            var cartItemsJson = HttpContext.Session.GetString("CartItems");
            CartItems = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(cartItemsJson);
            List<SessionLineItemOptions> stripeProducts = CreateStripeList(CartItems);

            var domain = "http://localhost:5087";
            var options = new SessionCreateOptions
            {
                LineItems = stripeProducts,
                Mode = "payment",
                SuccessUrl = domain + "/success",
                CancelUrl = domain + "/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        private List<SessionLineItemOptions> CreateStripeList(List<ShoppingCartItem> cartItems)
        {
            return cartItems.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = item.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.ProductName,
                    },
                },
                Quantity = item.Quantity,
            }).ToList();
        }
    }
}
