using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services.Implementations;
using CosmeticsStore.Services.Interfaces;

namespace CosmeticsStore.WPF
{
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
                var orders = _orderService.GetAllOrders();
                dgOrders.ItemsSource = orders ?? new List<Order>();
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
