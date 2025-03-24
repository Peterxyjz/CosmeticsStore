using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Repositories;
using CosmeticsStore.Services.Implementations;
using CosmeticsStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CosmeticsStore.Service.Interfaces;

namespace CosmeticsStore.WPF
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private User? _currentUser;
        private readonly IOrderService _orderService;
        private Order _selectedOrder;
        // Lấy cái này để hiển thị thêm subtotal trong OrderDetails(tại bảng OrderDetail lưu mỗi price, quantity)
        // class CartItemViewModel này nằm bên CartWindow.xaml.cs
        private List<CartItemViewModel> _cartItems;
        private readonly IProductService _productService;

        public OrdersWindow()
        {
            InitializeComponent();

            // Get the current user from application state
            if (App.Current.Properties.Contains("CurrentUser"))
            {
                _currentUser = (User?)App.Current.Properties["CurrentUser"];
            }
            // Initialize repositories and services
            var context = new CosmeticsDbContext();

            var orderRepository = new OrderRepository(context);
            var orderDetailRepository = new OrderDetailRepository(context);
            var cartRepository = new CartRepository(context);
            var cartItemRepository = new CartItemRepository(context);
            var productRepository = new ProductRepository(context);

            _orderService = new OrderService(
                orderRepository,
                orderDetailRepository,
                cartRepository,
                cartItemRepository,
                productRepository);
            _productService = new ProductService(productRepository);

            LoadOrders();
        }

        // Hàm để load toàn bộ Orders từ trước đến nay mà Current User đã đặt
        private void LoadOrders()
        {
            try
            {
                // Get all orders for the current user
                var orders = _orderService.GetOrdersByUserId(_currentUser.UserId);

                // Sort orders by date (newest first)
                orders = orders.OrderByDescending(o => o.OrderDate).ToList();

                // Display orders in the list box
                // Hiển thị thông tin vắn tắt của toàn bộ Order của Current Usser
                lstOrders.ItemsSource = orders;

                // If there are orders, select the first one
                if (orders.Any())
                {
                    lstOrders.SelectedIndex = 0;
                }
                else
                {
                    // Clear details panel if no orders
                    ClearOrderDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearOrderDetails()
        {
            // Clear all order details fields
            txtOrderId.Text = string.Empty;
            txtOrderDate.Text = string.Empty;
            txtTotalAmount.Text = string.Empty;
            txtShippingAddress.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtPaymentMethod.Text = string.Empty;
            txtPaymentStatus.Text = string.Empty;
            lstOrderDetails.ItemsSource = null;
        }

        // Hàm để load Info của 1 Order khi chọn Order trong List Orders cũng như là hiển thị OrderDetails của nó(Aka Product trong lần Order này)
        private void LoadOrderDetails(Order order)
        {
            if (order == null) return;

            _selectedOrder = order;

            // Display order header information
                txtOrderId.Text = order.OrderId.ToString();
                txtOrderDate.Text = order.OrderDate.Value.ToString("MM/dd/yyyy HH:mm");
                txtTotalAmount.Text = $"${order.TotalAmount:F2}";
                txtShippingAddress.Text = order.ShippingAddress;
                txtStatus.Text = order.Status;
                txtPaymentMethod.Text = order.PaymentMethod;
                txtPaymentStatus.Text = order.PaymentStatus;
            // Get order details from the database
            var orderWithDetails = _orderService.GetOrderById(order.OrderId);
            
            // Display order details in the list
            if (orderWithDetails != null && orderWithDetails.OrderDetails != null)
            {
                //lstOrderDetails.ItemsSource = orderWithDetails.OrderDetails; làm thế này ko ra subtotal
                // lấy _cartItem để hiển thị thêm subtotal trong OrderDetails(tại bảng OrderDetail lưu mỗi price, quantity)
                _cartItems = new List<CartItemViewModel>();

                foreach (var item in orderWithDetails.OrderDetails)
                {
                    _cartItems.Add(new CartItemViewModel
                    {
                        ProductId = item.ProductId.Value,
                        ProductName = item.Product.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Price * item.Quantity,
                    });
                }
                lstOrderDetails.ItemsSource = _cartItems;
            }
        }

        // khi chọn Order khác trong List Orders thì sẽ reload hiển thị Info của Order được chọn đó
        private void lstOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstOrders.SelectedItem is Order selectedOrder)
            {
                LoadOrderDetails(selectedOrder);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}