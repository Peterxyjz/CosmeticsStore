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
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        public string ShippingAddress { get; private set; }
        public string PaymentMethod { get; private set; }

        public CheckoutWindow()
        {
            InitializeComponent();

            // Initialize payment methods
            cmbPaymentMethod.ItemsSource = new List<string>
            {
                "Credit Card",
                "PayPal",
                "Bank Transfer",
                "Cash on Delivery",
                "Momo"
            };
            cmbPaymentMethod.SelectedIndex = 0;
        }

        private void btnConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            // Validate shipping address
            if (string.IsNullOrWhiteSpace(txtShippingAddress.Text))
            {
                MessageBox.Show("Please enter a valid shipping address.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate payment method
            if (cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Set properties for the caller to access
            ShippingAddress = txtShippingAddress.Text;
            PaymentMethod = cmbPaymentMethod.SelectedItem.ToString();

            // Set dialog result to true (confirmed)
            DialogResult = true;

            // Close the window
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Set dialog result to false (canceled)
            DialogResult = false;

            // Close the window
            Close();
        }
    }
}
