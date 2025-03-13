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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User? GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public void AddUser(User user)
        {
            // Validate user data
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                throw new ArgumentException("Username and password are required.");

            user.CreatedDate = DateTime.Now;
            user.Status = true;
            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                // Soft delete - just change status to inactive
                user.Status = false;
                _userRepository.Update(user);
            }
        }

        public IEnumerable<User> GetUsersByRole(string role)
        {
            return _userRepository.Find(u => u.Role == role && u.Status == true);
        }
    }
}