using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Service.Interfaces
{
    public interface IUserService
    {
        User? GetUserById(int id);
        IEnumerable<User> GetAllUsers();
        User? GetUserByUsername(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        IEnumerable<User> GetUsersByRole(string role);
    }
}