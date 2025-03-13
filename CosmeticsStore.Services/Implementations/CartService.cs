using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;

        public CartService(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public Cart GetActiveCartByUserId(int userId)
        {
            // Try to get an existing active cart
            var cart = _cartRepository.GetActiveCartByUserId(userId);

            // If no active cart exists, create a new one
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    Status = true // Active cart
                };
                _cartRepository.Add(cart);
            }

            return cart;
        }

        public void AddToCart(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            // Get the product to check availability and get price
            var product = _productRepository.GetById(productId);
            if (product == null)
                throw new ArgumentException("Product not found");

            // Check if there's enough stock
            if (product.StockQuantity < quantity)
                throw new InvalidOperationException("Not enough stock available");

            // Get active cart
            var cart = GetActiveCartByUserId(userId);

            // Check if the product is already in the cart
            var existingItem = _cartItemRepository.GetByCartId(cart.CartId)
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                // Update existing cart item
                existingItem.Quantity += quantity;
                _cartItemRepository.Update(existingItem);
            }
            else
            {
                // Add new cart item
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price // Store current price
                };
                _cartItemRepository.Add(cartItem);
            }
        }

        public void UpdateCartItem(int cartItemId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cartItem = _cartItemRepository.GetById(cartItemId);
            if (cartItem == null)
                throw new ArgumentException("Cart item not found");

            // Check product availability
            if (!cartItem.ProductId.HasValue)
                throw new ArgumentException("Product ID is missing in the cart item");

            var product = _productRepository.GetById(cartItem.ProductId.Value);
            if (product == null)
                throw new ArgumentException("Product not found");

            if (product.StockQuantity < quantity)
                throw new InvalidOperationException("Not enough stock available");

            // Update quantity
            cartItem.Quantity = quantity;
            _cartItemRepository.Update(cartItem);
        }

        public void RemoveFromCart(int cartItemId)
        {
            var cartItem = _cartItemRepository.GetById(cartItemId);
            if (cartItem == null)
                throw new ArgumentException("Cart item not found");

            _cartItemRepository.Remove(cartItem);
        }

        public void ClearCart(int cartId)
        {
            var cartItems = _cartItemRepository.GetByCartId(cartId);
            if (cartItems.Any())
            {
                _cartItemRepository.RemoveRange(cartItems);
            }
        }

        public decimal GetCartTotal(int cartId)
        {
            var cartItems = _cartItemRepository.GetByCartId(cartId);
            return cartItems.Sum(item => item.Price * item.Quantity);
        }

        public IEnumerable<CartItem> GetCartItems(int cartId)
        {
            return _cartItemRepository.GetByCartId(cartId);
        }
    }
}
