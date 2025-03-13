using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Login(string email, string password)
        {
            // In a real application, you would hash the password
            return _userRepository.Authenticate(email, password);
        }

        public bool Register(User user)
        {
            try
            {
                // Check if username already exists
                var existingUser = _userRepository.GetByUsername(user.Username);
                if (existingUser != null)
                {
                    return false;
                }

                // In a real application, you would hash the password
                user.CreatedDate = DateTime.Now;
                user.Status = true;
                user.Role = "Customer"; // Default role for new registrations
                _userRepository.Add(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}