using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using CosmeticsStore.Services.Implementations;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CosmeticsStore.WPF
{
    public partial class MainWindow : Window
    {
        private User? _currentUser;
        private readonly IProductService _productService;

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
            var context = new CosmeticsDbContext();
            var productRepository = new ProductRepository(context);
            _productService = new ProductService(productRepository);
            LoadProducts();
        }

        public void LoadProducts()
        {
            var products = _productService.GetAllProducts();

            foreach (Product product in products)
            {
                try
                {
                    if (string.IsNullOrEmpty(product.ImageUrl))
                    {
                        // Nếu không có ImageUrl, sử dụng default image
                        product.ImageUrl = "pack://application:,,,/CosmeticsStore.WPF;component/Images/default.jpg";
                    }
                    else if (!ImageExists(product.ImageUrl))   // tạo hàm ImageExists để kiểm tra
                    {
                        // Nếu ImageUrl không hợp lệ hoặc hình ảnh không tồn tại, sử dụng default image
                        product.ImageUrl = "pack://application:,,,/CosmeticsStore.WPF;component/Images/default.jpg";
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi và sử dụng hình ảnh mặc định
                    product.ImageUrl = "pack://application:,,,/CosmeticsStore.WPF;component/Images/default.jpg";
                }
            }

            lbProduct.ItemsSource = products;
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

        private void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        // hàm help cho loadProducts
        private bool ImageExists(string packUri)
        {
            try
            {
                // Tạo Uri từ chuỗi pack URI
                Uri uri = new Uri(packUri, UriKind.Absolute);

                // Sử dụng BitmapFrame để thử tải hình ảnh từ Pack URI
                BitmapFrame.Create(uri, BitmapCreateOptions.None, BitmapCacheOption.Default);

                // Nếu tạo được BitmapFrame mà không có exception, có nghĩa là hình ảnh tồn tại
                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Error checking image existence: {ex.Message}");
                return false;
            }
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            // Lấy button đã được click
            Button button = (Button)sender;

            // Lấy Id của sản phẩm từ Tag của button
            int productId = (int)button.Tag;

            // Lấy sản phẩm từ danh sách sản phẩm
            Product selectedProduct = _productService.GetProductById(productId);

            if (selectedProduct != null)
            {
                // Thêm sản phẩm vào giỏ hàng
                // Bạn cần có một service hoặc repository để xử lý giỏ hàng
                // Ví dụ: _cartService.AddToCart(_currentUser.Id, productId, 1);

                MessageBox.Show($"Added {selectedProduct.ProductName} to your cart!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}