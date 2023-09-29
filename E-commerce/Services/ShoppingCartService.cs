using E_commerce.Models;
using E_commerce.ViewModels;
using Newtonsoft.Json;

namespace E_commerce.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private List<ShoppingCartItem> _cartItems = new List<ShoppingCartItem>();

        public void AddToCart(Product product)
        {
            // Check if the product is already in the cart
            var existingItem = _cartItems.FirstOrDefault(item => item.ProductId == product.Id);

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
                _cartItems.Add(newItem);
            }
        }

        public void RemoveFromCart(int productId)
        {
            var itemToRemove = _cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                _cartItems.Remove(itemToRemove);
            }
        }

        public List<ShoppingCartItem> GetCartItems()
        {
            return _cartItems;
        }

        public decimal CalculateTotalPrice()
        {
            return _cartItems.Sum(item => item.Price * item.Quantity);
        }
    }
}
