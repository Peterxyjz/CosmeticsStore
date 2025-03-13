using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Service.Interfaces
{
    public interface IAuthService
    {
        User? Login(string email, string password);
        bool Register(User user);
    }
}