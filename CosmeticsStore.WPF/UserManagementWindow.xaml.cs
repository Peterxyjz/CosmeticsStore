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
    public partial class UserManagementWindow : Window
    {
        private readonly IUserService _userService;

        public UserManagementWindow()
        {
            InitializeComponent();
            var context = new CosmeticsDbContext();
            var userRepository = new UserRepository(context);
            _userService = new UserService(userRepository);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserList();
        }

        private void LoadUserList()
        {
            try
            {
                var users = _userService.GetAllUsers();
                dgUsers.ItemsSource = users ?? new List<User>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}");
            }
        }

        private void ResetInput()
        {
            txtUserID.Text = "";
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cboRole.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                var user = new User
                {
                    Username = txtUsername.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    Role = cboRole.Text,
                    Status = cboStatus.Text == "Active",
                    CreatedDate = DateTime.Now
                };

                _userService.AddUser(user);
                MessageBox.Show("User added successfully!");
                LoadUserList();
                ResetInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}");
            }
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User user)
            {
                txtUserID.Text = user.UserId.ToString();
                txtUsername.Text = user.Username;
                txtEmail.Text = user.Email;
                txtPassword.Text = user.Password;
                cboRole.Text = user.Role;
                cboStatus.SelectedIndex = user.Status.HasValue
      ? (user.Status.Value ? 0 : 1)
      : 0; // Hoặc bạn có thể chọn giá trị mặc định khác
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtUserID.Text, out int userId))
                {
                    var user = new User
                    {
                        UserId = userId,
                        Username = txtUsername.Text,
                        Email = txtEmail.Text,
                        Password = txtPassword.Text,
                        Role = cboRole.Text,
                        Status = cboStatus.Text == "Active"
                    };

                    _userService.UpdateUser(user);
                    MessageBox.Show("User updated successfully!");
                    LoadUserList();
                }
                else
                {
                    MessageBox.Show("Please select a user to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtUserID.Text, out int userId))
                {
                    _userService.DeleteUser(userId);
                    MessageBox.Show("User deleted successfully!");
                    LoadUserList();
                    ResetInput();
                }
                else
                {
                    MessageBox.Show("Please select a user to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}");
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
