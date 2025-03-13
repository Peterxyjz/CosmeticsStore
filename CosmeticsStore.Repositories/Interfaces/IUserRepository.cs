using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);
        User? Authenticate(string email, string password);
    }
}