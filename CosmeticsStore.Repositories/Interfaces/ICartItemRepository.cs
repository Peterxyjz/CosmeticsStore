using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        CartItem GetById(int id);
        IEnumerable<CartItem> GetByCartId(int cartId);
        void Add(CartItem cartItem);
        void Update(CartItem cartItem);
        void Remove(CartItem cartItem);
        void RemoveRange(IEnumerable<CartItem> cartItems);
    }
}
