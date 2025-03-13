using CosmeticsStore.Repositories.Models;
using System;
using System.Windows;

namespace CosmeticsStore.WPF
{
    public partial class MainWindow : Window
    {
        private User? _currentUser;

        public MainWindow()
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
            // Implementation for browsing products
            MessageBox.Show("Product browsing not implemented yet");
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for viewing cart
            MessageBox.Show("Cart view not implemented yet");
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for viewing orders
            MessageBox.Show("Orders view not implemented yet");
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