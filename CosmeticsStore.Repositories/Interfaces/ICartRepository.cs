using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Cart GetById(int id);
        Cart GetActiveCartByUserId(int userId);
        void Add(Cart cart);
        void Update(Cart cart);
    }
}