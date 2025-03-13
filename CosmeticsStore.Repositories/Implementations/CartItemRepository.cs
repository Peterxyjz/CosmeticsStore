using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Implementations
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly CosmeticsDbContext _context;

        public CartItemRepository(CosmeticsDbContext context)
        {
            _context = context;
        }

        public CartItem GetById(int id)
        {
            return _context.CartItems.Find(id);
        }

        public IEnumerable<CartItem> GetByCartId(int cartId)
        {
            return _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cartId)
                .ToList();
        }

        public void Add(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
        }

        public void Update(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            _context.SaveChanges();
        }

        public void Remove(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }
}
