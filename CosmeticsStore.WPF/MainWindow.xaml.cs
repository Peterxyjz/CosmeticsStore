using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using CosmeticsStore.Services.Implementations;
using CosmeticsStore.Services.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CosmeticsStore.WPF
{
    public partial class MainWindow : Window
    {
        private User? _currentUser;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;


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
            var cartRepository = new CartRepository(context);
            var cartItemRepository = new CartItemRepository(context);
            var orderRepository = new OrderRepository(context);
            var orderDetailRepository = new OrderDetailRepository(context);

            _cartService = new CartService(cartRepository, cartItemRepository, productRepository);
            _productService = new ProductService(productRepository);
            _orderService = new OrderService(orderRepository, orderDetailRepository, cartRepository, cartItemRepository, productRepository);

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
            try
            {
                // Lấy Cart hiện tại của current User, Nếu User chưa có Cart thì tạo mới
                Cart userCart = _cartService.GetActiveCartByUserId(_currentUser.UserId);

                // Lấy CartItems trong Cart
                IEnumerable<CartItem> cartItems = _cartService.GetCartItems(userCart.CartId);

                // Khi Click Cart thì kiểm tra coi Cart có trống không(Aka Có Cart Items nào Ở trong Cart ko)
                // Nếu trống thì return luôn
                if (!cartItems.Any())
                {
                    MessageBox.Show("Your cart is empty", "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Mở CartWindow để hiển thị Cart
                // Truyền Parameter thông qua Constructor
                // Truyền CartId để lát qua bên Cart Window không cần phải viết lại đoạn code lấy Cart của Current User
                CartWindow cartWindow = new CartWindow(userCart.CartId);
                cartWindow.Owner = this;
                cartWindow.ShowDialog();

                // Cập nhật lại List Product sau khi CartWindow đóng???
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cart: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // IMPLEMENT FUNCTION NÀY
        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                // Lấy các Order mà User hiện tại đã Commit
                var userOrders = _orderService.GetOrdersByUserId(_currentUser.UserId);

                // Nếu User chưa có Order thì sẽ không mở được OrderWindow
                if (userOrders == null || !userOrders.Any())
                {
                    MessageBox.Show("You don't have any orders yet", "No Orders",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Open Orders Window
                OrdersWindow ordersWindow = new OrdersWindow();
                ordersWindow.Owner = this;
                ordersWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        // hàm add to cart
        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {

            // Lấy button đã được click(button add to cart)
            Button button = (Button)sender;

            // Lấy Id của sản phẩm từ Tag của button(khá giống hidden control)
            int productId = (int)button.Tag;

            // Lấy sản phẩm từ danh sách sản phẩm
            Product selectedProduct = _productService.GetProductById(productId);

            if (selectedProduct != null)
            {
                try
                {
                    // Kiểm tra xem sản phẩm còn hàng không
                    if (selectedProduct.StockQuantity <= 0)
                    {
                        MessageBox.Show($"{selectedProduct.ProductName} is out of stock!", "Out of Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Thêm sản phẩm vào giỏ hàng
                    _cartService.AddToCart(_currentUser.UserId, productId, 1);

                    MessageBox.Show($"Added {selectedProduct.ProductName} to your cart!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Hỏi người dùng có muốn xem giỏ hàng không
                    // Nếu có thì Open Cart Window(Gọi hàm Click Cart)
                    if (MessageBox.Show("View your cart now?", "Cart", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        btnCart_Click(this, new RoutedEventArgs());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding product to cart: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Product not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}