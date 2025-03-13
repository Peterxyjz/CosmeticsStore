using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using CosmeticsStore.Services.Implementations;
using System;
using System.Windows;

namespace CosmeticsStore.WPF
{
    public partial class RegisterWindow : Window
    {
        private readonly IAuthService _authService;

        public RegisterWindow()
        {
            InitializeComponent();

            // Initialize services
            var context = new CosmeticsDbContext();
            var userRepository = new UserRepository(context);
            _authService = new AuthService(userRepository);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Basic validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                txtMessage.Text = "Please fill in all fields";
                return;
            }

            if (password != confirmPassword)
            {
                txtMessage.Text = "Passwords do not match";
                return;
            }

            // Create user object
            User newUser = new User
            {
                Username = username,
                Email = email,
                Password = password,
                Role = "Customer" // Set role explicitly
            };

            // Register user
            bool success = _authService.Register(newUser);

            if (success)
            {
                MessageBox.Show("Registration successful! Please login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                txtMessage.Text = "Username or email already exists";
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void txtLogin_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}