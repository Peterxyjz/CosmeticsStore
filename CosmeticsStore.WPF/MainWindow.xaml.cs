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
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CosmeticsStore.WPF
{
    public partial class MainWindow : Window
    {
        private User? _currentUser;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        //private Popup categoryPopup;
        //private ListBox categoryListBox;


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
            var categoryRepository = new CategoryRepository(context);

            _cartService = new CartService(cartRepository, cartItemRepository, productRepository);
            _productService = new ProductService(productRepository);
            _orderService = new OrderService(orderRepository, orderDetailRepository, cartRepository, cartItemRepository, productRepository);
            _categoryService = new CategoryService(categoryRepository);

            // Khởi tạo popup danh mục
            //InitializeCategoryPopup();
            // B1: khởi tạo popup bằng hàm InitializeCategoryPopup()
            // B2: khi click vào button Category thì kích hoạt btnCategories_Click()
            // B3: Trong hàm btnCategories_Click() sẽ gọi hàm LoadCategories() để load category
            // B4: Trong hàm LoadCategories() sẽ lấy danh sách danh mục từ CategoryService và thêm vào ListBox
            // B5: Khi chọn 1 danh mục thì sẽ gọi hàm CategoryListBox_SelectionChanged() để filter product theo category đã chọn
            // code trong method InitializaCategoryPopup() về cơ bản có thể nằm bên phần giao diện

            LoadProducts();
        }


        //// hàm khởi tạo popup cho category, chơi kiểu nhúng giao diện vào code
        //private void InitializeCategoryPopup()
        //{
        //    // Tạo Popup control
        //    categoryPopup = new Popup();
        //    categoryPopup.AllowsTransparency = true;
        //    categoryPopup.PopupAnimation = PopupAnimation.Fade;
        //    categoryPopup.StaysOpen = false;

        //    // Tạo Border cho popup để có thể style
        //    Border border = new Border();
        //    border.BorderBrush = new SolidColorBrush(Colors.LightGray);
        //    border.BorderThickness = new Thickness(1);
        //    border.Background = new SolidColorBrush(Colors.White);
        //    border.CornerRadius = new CornerRadius(4);
        //    border.Padding = new Thickness(2);
        //    border.Effect = new System.Windows.Media.Effects.DropShadowEffect
        //    {
        //        Color = Colors.Gray,
        //        Direction = 315,
        //        ShadowDepth = 3,
        //        BlurRadius = 5,
        //        Opacity = 0.3
        //    };

        //    // Stack panel để chứa tiêu đề và ListBox
        //    StackPanel panel = new StackPanel();
        //    panel.Width = 200;
        //    panel.Background = new SolidColorBrush(Colors.White);

        //    // Thêm Title 

        //    // Tạo Border
        //    Border titleBorder = new Border();
        //    titleBorder.BorderBrush = new SolidColorBrush(Colors.LightGray);
        //    titleBorder.BorderThickness = new Thickness(0, 0, 0, 1);
        //    titleBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF5F5F5"));

        //    // Tạo TextBlock
        //    TextBlock titleBlock = new TextBlock();
        //    titleBlock.Text = "Select Category";
        //    titleBlock.FontWeight = FontWeights.SemiBold;
        //    titleBlock.Padding = new Thickness(8, 8, 8, 4);

        //    // Thêm TextBlock vào Border
        //    titleBorder.Child = titleBlock;

        //    // Thêm Border vào panel
        //    panel.Children.Add(titleBorder);

        //    // Tạo ListBox cho Category
        //    categoryListBox = new ListBox();
        //    categoryListBox.MaxHeight = 300;
        //    categoryListBox.BorderThickness = new Thickness(0);
        //    // Ủy quyền event aka giống khai báo event trong giao diện xaml
        //    categoryListBox.SelectionChanged += CategoryListBox_SelectionChanged;
        //    panel.Children.Add(categoryListBox);

        //    // Thêm "All Categories" vào danh sách dropdown(thành 1 item của category List Box)
        //    ListBoxItem allCategoriesItem = new ListBoxItem();
        //    allCategoriesItem.Content = "All Categories";
        //    allCategoriesItem.Padding = new Thickness(8, 5, 8, 5);
        //    allCategoriesItem.Tag = 0; // Tag = 0 để đánh dấu là "All Categories"
        //    categoryListBox.Items.Add(allCategoriesItem);

        //    // Thêm separator
        //    Separator separator = new Separator();
        //    separator.Margin = new Thickness(5, 2, 5, 2);
        //    panel.Children.Add(separator);

        //    // Thêm nút Close
        //    Button closeButton = new Button();
        //    closeButton.Content = "Close";
        //    closeButton.Margin = new Thickness(5);
        //    closeButton.HorizontalAlignment = HorizontalAlignment.Right;
        //    closeButton.Click += (s, e) => categoryPopup.IsOpen = false;
        //    panel.Children.Add(closeButton);

        //    border.Child = panel;
        //    categoryPopup.Child = border;
        //}

        // khi Click vào button thì sổ ra ListBox chứa các Category
        //private void btnCategories_Click(object sender, RoutedEventArgs e)
        //{
        //    // Load Category lên MainWindow(dropdown xuống)
        //    LoadCategories();

        //    // Xác định vị trí để hiển thị popup(Cái List Box) dropdown
        //    var btnCategories = sender as Button;
        //    categoryPopup.PlacementTarget = btnCategories;
        //    categoryPopup.Placement = PlacementMode.Bottom;

        //    // Hiển thị popup ListBox chứa Category dropdown(từ chỗ này cái List Box mới hiện trên màn hình)
        //    // giống MainWindow.Show()
        //    // trước đó chỉ giống như là MainWindow main = new MainWindow() hoi
        //    categoryPopup.IsOpen = true;
        //}

        //private void LoadCategories()
        //{
        //    try
        //    {
        //        // Xóa tất cả các Category hiện tại (trừ "All Categories")
        //        while (categoryListBox.Items.Count > 1)
        //        {
        //            categoryListBox.Items.RemoveAt(1);
        //        }

        //        // Lấy toàn bộ Category dưới db
        //        var categories = _categoryService.GetAllCategories();

        //        // Thêm các Category vào ListBox
        //        foreach (var category in categories)
        //        {
        //            ListBoxItem item = new ListBoxItem();
        //            item.Content = category.CategoryName;
        //            item.Padding = new Thickness(8, 5, 8, 5);
        //            item.Tag = category.CategoryId; // Lưu CategoryId trong Tag
        //            categoryListBox.Items.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
        //            MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}


        //// hàm xử lí khi chọn Category để filter Product khi load lên
        //private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (categoryListBox.SelectedItem is ListBoxItem selectedItem)
        //    {
        //        // Lấy CategoryId từ Tag của item được chọn
        //        int categoryId = (int)selectedItem.Tag;

        //        // Filter Product theo Category đã chọn
        //        LoadProducts(categoryId);

        //        // Đóng popup
        //        categoryPopup.IsOpen = false;
        //    }
        //}


        // ĐỐNG TRÊN LÀ NHÉT GIAO DIỆN VÀO CODE, NÊN TÁCH GIAO DIỆN VÀ PHẦN XỬ LÍ RA



        // khi Click vào button thì sổ ra ListBox chứa các Category
        private void btnCategories_Click(object sender, RoutedEventArgs e)
        {
            // Load Category dưới db lên Popup Listbox để chuẩn bị cho việc Show
            LoadCategories();

            // Show cái Popup Listbox toàn bộ Category
            categoryPopup.IsOpen = true;
        }

        private void LoadCategories()
        {
            try
            {
                // Clear all categories except the "All Categories" item at index 0
                categoryListBox.Items.Clear();
                
                // Add "All Categories" item first
                ListBoxItem allCategoryItem = new ListBoxItem();
                allCategoryItem.Content = "All Categories";
                allCategoryItem.Padding = new Thickness(15, 10, 15, 10);
                allCategoryItem.Tag = 0;
                categoryListBox.Items.Add(allCategoryItem);

                // Lấy Category dưới db
                var categories = _categoryService.GetAllCategories();

                // nhồi Category vào ListBox
                foreach (var category in categories)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = category.CategoryName;
                    item.Padding = new Thickness(15, 10, 15, 10);
                    item.Tag = category.CategoryId; // Store CategoryId in Tag
                    categoryListBox.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoryListBox.SelectedItem is ListBoxItem selectedItem)
            {
                // Make sure the Tag property exists and can be converted to an integer
                if (selectedItem.Tag != null)
                {
                    int categoryId;
                    if (int.TryParse(selectedItem.Tag.ToString(), out categoryId))
                    {
                        // Get the category name from the selected item
                        string categoryName = selectedItem.Content.ToString();

                        // Update the featured products text based on selected category
                        if (categoryId > 0) // If not "All Categories"
                        {
                            txtFeaturedProducts.Text = $"{categoryName} Products";
                        }
                        else
                        {
                            txtFeaturedProducts.Text = "Featured Products";
                        }

                        // Filter products by the selected category
                        LoadProducts(categoryId);
                    }
                    else
                    {
                        // If tag is not a valid integer, load all products
                        txtFeaturedProducts.Text = "Featured Products";
                        LoadProducts(null);
                    }
                }
                else
                {
                    // If tag is null, load all products
                    txtFeaturedProducts.Text = "Featured Products";
                    LoadProducts(null);
                }

                // Close the popup
                categoryPopup.IsOpen = false;
            }
        }

        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            categoryPopup.IsOpen = false;
        }



        public void LoadProducts(int? categoryId = null)
        {
            //var products = _productService.GetAllProducts();

            // Lấy ProductId dựa trên filter Category
            IEnumerable<Product> products;
            if (categoryId.HasValue && categoryId.Value > 0) // Kiểm tra categoryId > 0 để loại trừ trường hợp "All Categories"
            {
                products = _productService.GetProductsByCategory(categoryId.Value);
            }
            else
            {
                products = _productService.GetAllProducts();
            }

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
                //LoadProducts();
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