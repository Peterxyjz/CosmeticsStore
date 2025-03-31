using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CosmeticsStore.Repositories.Implementations;
using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using CosmeticsStore.Services.Implementations;

namespace CosmeticsStore.WPF
{
    public partial class ProductManagementWindow : Window
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        // Constructor nhận dependency từ DI
        public ProductManagementWindow()
        {
            InitializeComponent();
            var context = new CosmeticsDbContext();

            var productRepository = new ProductRepository(context);
            var cartRepository = new CartRepository(context);
            var cartItemRepository = new CartItemRepository(context);
            var orderRepository = new OrderRepository(context);
            var orderDetailRepository = new OrderDetailRepository(context);
            var categoryRepository = new CategoryRepository(context);

            _productService = new ProductService(productRepository);
           
            _categoryService = new CategoryService(categoryRepository);
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategoryList();
            LoadProductList();
        }

        private void LoadCategoryList()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                cboCategory.ItemsSource = categories;
                cboCategory.DisplayMemberPath = "CategoryName";
                cboCategory.SelectedValuePath = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}");
            }
        }

        private void LoadProductList()
        {
            try
            {
                var products = _productService.GetAllProducts();
                dgData.ItemsSource = products ?? new List<Product>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
        }

        private void ResetInput()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtDescription.Text = "";
            txtPrice.Text = "";
            txtUnitsInStock.Text = "";
            txtImageUrl.Text = "";
            cboCategory.SelectedValue = null;
            cboStatus.SelectedIndex = 0;
            txtCreatedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtUnitsInStock.Text) ||
                    cboCategory.SelectedValue == null)
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                var selectedItem = cboStatus.SelectedItem as ComboBoxItem;
                bool status = selectedItem?.Tag?.ToString() == "true";

                var product = new Product
                {
                    ProductName = txtProductName.Text,
                    Description = txtDescription.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    StockQuantity = short.Parse(txtUnitsInStock.Text),
                    CategoryId = (int)cboCategory.SelectedValue,
                    ImageUrl = txtImageUrl.Text,
                    CreatedDate = DateTime.Now,
                    Status = status
                };

                _productService.AddProduct(product);
                MessageBox.Show("Product added successfully!");
                LoadProductList();
                ResetInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}");
            }
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem is Product product)
            {
                txtProductID.Text = product.ProductId.ToString();
                txtProductName.Text = product.ProductName;
                txtDescription.Text = product.Description;
                txtPrice.Text = product.Price.ToString();
                txtUnitsInStock.Text = product.StockQuantity.ToString();
                cboCategory.SelectedValue = product.CategoryId;
                txtImageUrl.Text = product.ImageUrl;
                txtCreatedDate.Text = product.CreatedDate.HasValue
    ? product.CreatedDate.Value.ToString("yyyy-MM-dd")
    : DateTime.Now.ToString("yyyy-MM-dd");
                cboStatus.SelectedIndex = product.Status.HasValue
    ? (product.Status.Value ? 0 : 1)
    : 0; // Hoặc bạn có thể chọn giá trị mặc định khác
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtProductID.Text, out int productId))
                {
                    var selectedItem = cboStatus.SelectedItem as ComboBoxItem;
                    bool status = selectedItem?.Tag?.ToString() == "true";

                    var product = new Product
                    {
                        ProductId = productId,
                        ProductName = txtProductName.Text,
                        Description = txtDescription.Text,
                        Price = decimal.Parse(txtPrice.Text),
                        StockQuantity = short.Parse(txtUnitsInStock.Text),
                        CategoryId = (int)cboCategory.SelectedValue,
                        ImageUrl = txtImageUrl.Text,
                        CreatedDate = DateTime.Parse(txtCreatedDate.Text),
                        Status = status
                    };

                    _productService.UpdateProduct(product);
                    MessageBox.Show("Product updated successfully!");
                    LoadProductList();
                }
                else
                {
                    MessageBox.Show("Please select a product to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtProductID.Text, out int productId))
                {
                    _productService.DeleteProduct(productId);
                    MessageBox.Show("Product deleted successfully!");
                    LoadProductList();
                    ResetInput();
                }
                else
                {
                    MessageBox.Show("Please select a product to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow(); // Tạo cửa sổ AdminWindow
            adminWindow.Show(); // Hiển thị AdminWindow
            this.Close(); // Đóng cửa sổ hiện tại
        }
    }
}
