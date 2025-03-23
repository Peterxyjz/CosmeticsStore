using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Service.Interfaces;
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

namespace CosmeticsStore.WPF
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private readonly int _cartId;
        private readonly ICartService _cartService;
        private List<CartItemViewModel> _cartItems;

        public CartWindow(int cartId)
        {
            InitializeComponent();

            _cartId = cartId;

            var context = new CosmeticsDbContext();

            CartRepository cartRepository = new CartRepository(context);
            ProductRepository productRepository = new ProductRepository(context);
            CartItemRepository cartItemRepository = new CartItemRepository(context);

            _cartService = new CartService(cartRepository, cartItemRepository, productRepository);

            LoadCartItems();
            UpdateTotalPrice();
        }


        // Hàm LoadCartItems
        // Sẽ được gọi khi khởi tạo CartWindow và sau khi thêm, xóa, cập nhật số lượng sản phẩm để Reload ListView
        private void LoadCartItems()
        {
            try
            {
                // Lấy danh sách các sản phẩm trong giỏ hàng
                var cartItems = _cartService.GetCartItems(_cartId);

                // Chuyển đổi CartItem thành CartItemViewModel để hiển thị
                _cartItems = new List<CartItemViewModel>();

                foreach (var item in cartItems)
                {
                    _cartItems.Add(new CartItemViewModel
                    {
                        CartItemId = item.CartItemId,
                        ProductId = item.ProductId.Value,
                        ProductName = item.Product.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Price * item.Quantity,
                        ImageUrl = item.Product.ImageUrl

                    });
                }

                // Hiển thị danh sách trên ListView
                lvCartItems.ItemsSource = _cartItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cart items: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Hàm Update Total Price
        // Sẽ được gọi thường xuyên sau khi thêm, xóa, cập nhật số lượng sản phẩm
        private void UpdateTotalPrice()
        {
            // Sau khi thêm, xóa, cập nhật số lượng sản phẩm, nhớ load lại ListView và cập nhật lại tổng tiền
            decimal total = _cartService.GetCartTotal(_cartId);
            txtTotal.Text = $"Total: ${total:F2}";
        }

        private void btnIncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItemViewModel)button.DataContext;

            try
            {
                _cartService.UpdateCartItem(cartItem.CartItemId, cartItem.Quantity + 1);
                // Increase Xong phải Load lại CartItems và cập nhật lại Total Price
                LoadCartItems();
                UpdateTotalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItemViewModel)button.DataContext;

            if (cartItem.Quantity > 1)
            {
                try
                {
                    _cartService.UpdateCartItem(cartItem.CartItemId, cartItem.Quantity - 1);
                    // Decrease Xong phải Load lại CartItems và cập nhật lại Total Price
                    LoadCartItems();
                    UpdateTotalPrice();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Nếu số lượng là 1, hỏi User có muốn xóa Product khỏi Cart không
                if (MessageBox.Show("Remove this item from cart?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    RemoveCartItem(cartItem.CartItemId);
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var cartItem = (CartItemViewModel)button.DataContext;

            if (MessageBox.Show("Remove this item from cart?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RemoveCartItem(cartItem.CartItemId);
            }
        }

        // hàm helper cho btnRemove_Click và btnDecreaseQuantity_Click
        private void RemoveCartItem(int cartItemId)
        {
            try
            {
                _cartService.RemoveFromCart(cartItemId);
                // Remove Xong phải Load lại CartItems và cập nhật lại Total Price
                LoadCartItems();
                UpdateTotalPrice();

                // Nếu cart trống, Close CartWindow luôn
                if (!_cartItems.Any())
                {
                    MessageBox.Show("Your cart is now empty", "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Chưa Implemented
        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Proceed to checkout?", "Checkout", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Chưa implemented chức năng checkout
                MessageBox.Show("Checkout functionality will be implemented soon.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnContinueShopping_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // ViewModel để hiển thị trên giao diện
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public string ImageUrl { get; set; }
    }
}
