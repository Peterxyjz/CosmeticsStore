using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using CosmeticsStore.Services.Implementations;
using System;
using System.Windows;

namespace CosmeticsStore.WPF
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();

            // Initialize services
            var context = new CosmeticsDbContext();
            var userRepository = new UserRepository(context);
            _authService = new AuthService(userRepository);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                txtMessage.Text = "Please enter email and password";
                return;
            }

            User? user = _authService.Login(email, password);

            if (user != null)
            {
                // Store user in application state
                App.Current.Properties["CurrentUser"] = user;

                // Navigate based on role
                if (user.Role == "Manager")
                {
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }

                this.Close();
            }
            else
            {
                txtMessage.Text = "Invalid email or password";
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void txtRegister_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}