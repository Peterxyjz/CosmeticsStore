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
    public class CartRepository : ICartRepository
    {
        private readonly CosmeticsDbContext _context;

        public CartRepository(CosmeticsDbContext context)
        {
            _context = context;
        }

        public Cart GetById(int id)
        {
            return _context.Carts.Find(id);
        }

        public Cart GetActiveCartByUserId(int userId)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId && c.Status == true);
        }

        public void Add(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }
    }
}