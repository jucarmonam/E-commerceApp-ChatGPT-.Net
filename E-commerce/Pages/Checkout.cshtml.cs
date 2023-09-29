using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace E_commerce.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private string PublicKey => _configuration.GetSection("StripeSettings")["PublishableKey"];

        public CheckoutModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnPost()
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

            var options = new ChargeCreateOptions
            {
                Amount = 1000,  // Amount in cents (e.g., $10.00)
                Currency = "usd",
                Description = "Example charge",
                //Source = Request.Form["stripeToken"]
            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            // Handle the charge response and update your database as needed.

            return RedirectToPage("/OrderConfirmation"); // Redirect to a confirmation page.
        }
    }
}
