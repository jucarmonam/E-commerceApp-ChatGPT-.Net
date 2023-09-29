using Microsoft.AspNetCore.Identity;

namespace E_commerce.Models
{
    public class User : IdentityUser
    {
        // Your custom properties here
        // Note: You don't need to include properties like Id, Username, Email, and Password.
        // ASP.NET Core Identity provides those properties.

        // Navigation property for the Orders relationship
        public List<Order>? Orders { get; set; }
    }
}
