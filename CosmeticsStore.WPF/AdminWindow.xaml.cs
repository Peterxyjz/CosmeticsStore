using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using CosmeticsStore.Services.Implementations;
using CosmeticsStore.Services.Interfaces;
using System;
using System.Windows;

namespace CosmeticsStore.WPF
{
    public partial class AdminWindow : Window
    {
        private User? _currentUser;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        public AdminWindow()
        {
            InitializeComponent();
            var context = new CosmeticsDbContext();
            var userRepository = new UserRepository(context);
            var productRepository = new ProductRepository(context);
            var cartRepository = new CartRepository(context);
            var cartItemRepository = new CartItemRepository(context);
            var orderRepository = new OrderRepository(context);
            var orderDetailRepository = new OrderDetailRepository(context);
            var categoryRepository = new CategoryRepository(context);
            _userService = new UserService(userRepository);
            _cartService = new CartService(cartRepository, cartItemRepository, productRepository);
            _productService = new ProductService(productRepository);
            _orderService = new OrderService(orderRepository, orderDetailRepository, cartRepository, cartItemRepository, productRepository);
            _categoryService = new CategoryService(categoryRepository);
            // Get the current user from application state
            if (App.Current.Properties.Contains("CurrentUser"))
            {
                _currentUser = (User?)App.Current.Properties["CurrentUser"];
                if (_currentUser != null)
                {
                    txtWelcome.Text = $"Welcome, {_currentUser.Username}";
                }
            }

            UpdateTotalProducts(); // Call this method to update the total products on load
            UpdateTotalUser();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            // Already on dashboard, no need to navigate
            // Could refresh dashboard data if needed
            UpdateTotalProducts();
            UpdateTotalUser();
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Product Management
            ProductManagementWindow productManagementWindow = new ProductManagementWindow();
            productManagementWindow.Show();
            this.Close();
        }

    

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            OrderManagementWindow orderManagementWindow = new OrderManagementWindow();
            orderManagementWindow.Show();
            this.Close();
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow userManagementWindow = new UserManagementWindow();
            userManagementWindow.Show();
            this.Close();
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

        private void UpdateTotalProducts()
        {
            var totalProducts = _productService.GetTotalProducts(); // Assuming you have this method in your service
            txtTotalProducts.Text = totalProducts.ToString();
        }
        private void UpdateTotalUser()
        {
            var totalUsers = _userService.GetTotalUsers(); // Assuming you have this method in your service
            txtTotalUsers.Text = totalUsers.ToString();
        }
    }
}