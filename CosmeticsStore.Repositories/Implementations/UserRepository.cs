using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CosmeticsDbContext context) : base(context)
        {
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User? Authenticate(string email, string password)
        {
            // In a real application, you would hash the password and compare hashes
            return _context.Users.FirstOrDefault(u =>
                u.Email == email &&
                u.Password == password &&
                u.Status == true);
        }
    }
}