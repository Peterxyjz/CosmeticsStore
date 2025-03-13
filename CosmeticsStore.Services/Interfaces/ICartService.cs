using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;

namespace CosmeticsStore.Services.Interfaces
{
    public interface ICartService
    {
        // Get active cart for a user
        Cart GetActiveCartByUserId(int userId);

        // Add a product to the user's cart
        void AddToCart(int userId, int productId, int quantity);

        // Update quantity of an item in the cart
        void UpdateCartItem(int cartItemId, int quantity);

        // Remove an item from the cart
        void RemoveFromCart(int cartItemId);

        // Clear all items from a cart
        void ClearCart(int cartId);

        // Get the total price of items in the cart
        decimal GetCartTotal(int cartId);

        // Get all items in a cart with product details
        IEnumerable<CartItem> GetCartItems(int cartId);
    }
}