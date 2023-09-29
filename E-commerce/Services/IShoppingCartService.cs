using E_commerce.Models;
using E_commerce.ViewModels;

namespace E_commerce.Services
{
    public interface IShoppingCartService
    {
        void AddToCart(Product product);
        void RemoveFromCart(int productId);
        List<ShoppingCartItem> GetCartItems();

        decimal CalculateTotalPrice();
    }
}
