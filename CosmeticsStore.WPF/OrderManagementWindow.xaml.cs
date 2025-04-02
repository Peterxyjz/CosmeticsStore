using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services.Implementations;
using CosmeticsStore.Services.Interfaces;

namespace CosmeticsStore.WPF
{
    // Converter for Order Status Colors
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "Processing":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE3B02B")); // Orange
                    case "Shipped":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3498DB")); // Blue
                    case "Delivered":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF27AE60")); // Green
                    case "Cancelled":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE74C3C")); // Red
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF95A5A6")); // Gray
                }
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for Payment Status Colors
    public class PaymentStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string paymentStatus)
            {
                switch (paymentStatus)
                {
                    case "Paid":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF27AE60")); // Green
                    case "Unpaid":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE74C3C")); // Red
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF95A5A6")); // Gray
                }
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class OrderManagementWindow : Window
    {
        private readonly IOrderService _orderService;
        private Order _selectedOrder; // Biến lưu trữ đơn hàng được chọn

        public OrderManagementWindow()
        {
            InitializeComponent();
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrderList();
        }

        private void LoadOrderList()
        {
            try
            {
                var orders = _orderService.GetAllOrders().ToList();
                dgOrders.ItemsSource = orders ?? new List<Order>();

                // Update summary counts
                if (orders != null)
                {
                    txtTotalOrders.Text = orders.Count.ToString();
                    txtProcessingOrders.Text = orders.Count(o => o.Status == "Processing").ToString();
                    txtShippedOrders.Text = orders.Count(o => o.Status == "Shipped").ToString();
                    txtDeliveredOrders.Text = orders.Count(o => o.Status == "Delivered").ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}");
            }
        }

        private void dgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem is Order selectedOrder)
            {
                _selectedOrder = selectedOrder;
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Please select an order.");
                return;
            }

            if (_selectedOrder.Status == "Processing")
            {
                _orderService.AcceptOrder(_selectedOrder.OrderId);
                MessageBox.Show("Order accepted successfully!");
                LoadOrderList();
            }
            else
            {
                MessageBox.Show("Only processing orders can be accepted.");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Please select an order.");
                return;
            }

            if (_selectedOrder.Status == "Pending" || _selectedOrder.Status == "Processing")
            {
                _orderService.CancelOrder(_selectedOrder.OrderId);
                MessageBox.Show("Order cancelled successfully!");
                LoadOrderList();
            }
            else
            {
                MessageBox.Show("Only pending or processing orders can be cancelled.");
            }
        }

        private void btnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Please select an order.");
                return;
            }

            _orderService.UpdateOrderStatus(_selectedOrder.OrderId);
            MessageBox.Show("Order status updated successfully!");
            LoadOrderList();
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow(); // Tạo cửa sổ AdminWindow
            adminWindow.Show(); // Hiển thị AdminWindow
            this.Close(); // Đóng cửa sổ hiện tại
        }
    }
}
