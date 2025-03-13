using CosmeticsStore.Repositories.Models;
using System;
using System.Windows;

namespace CosmeticsStore.WPF
{
    public partial class AdminWindow : Window
    {
        private User? _currentUser;

        public AdminWindow()
        {
            InitializeComponent();

            // Get the current user from application state
            if (App.Current.Properties.Contains("CurrentUser"))
            {
                _currentUser = (User?)App.Current.Properties["CurrentUser"];
                if (_currentUser != null)
                {
                    txtWelcome.Text = $"Welcome, {_currentUser.Username}";
                }
            }
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for product management
            MessageBox.Show("Product management not implemented yet");
        }

        private void btnCategories_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for category management
            MessageBox.Show("Category management not implemented yet");
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for order management
            MessageBox.Show("Order management not implemented yet");
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for user management
            MessageBox.Show("User management not implemented yet");
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Clear the current user
            if (App.Current.Properties.Contains("CurrentUser"))
            {
                App.Current.Properties.Remove("CurrentUser");
            }

            // Navigate back to login
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}