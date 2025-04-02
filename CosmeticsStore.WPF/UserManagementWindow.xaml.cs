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
        private bool isEditMode = false;

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
            ResetInput();
            SetCreateMode();
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
            txtPassword.Password = "";
            cboRole.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
        }

        private void SetCreateMode()
        {
            isEditMode = false;
            txtUsername.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtPassword.IsEnabled = true;
            btnCreate.Content = "Create";
            btnUpdate.IsEnabled = false;
            ResetInput();
        }

        private void SetEditMode()
        {
            isEditMode = true;
            txtUsername.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtPassword.IsEnabled = false;
            btnUpdate.IsEnabled = true;
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User user)
            {
                txtUserID.Text = user.UserId.ToString();
                txtUsername.Text = user.Username;
                // Note: Password is not shown for security reasons
                txtPassword.Password = "********"; // Masked password
                txtEmail.Text = user.Email;
                cboRole.Text = user.Role;
                cboStatus.SelectedIndex = user.Status.HasValue
                    ? (user.Status.Value ? 0 : 1)
                    : 0;

                SetEditMode();
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                var user = new User
                {
                    Username = txtUsername.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Password,
                    Role = cboRole.Text,
                    Status = cboStatus.Text == "Active",
                    CreatedDate = DateTime.Now
                };

                _userService.AddUser(user);
                MessageBox.Show("User added successfully!");
                LoadUserList();
                ResetInput();
                SetCreateMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtUserID.Text, out int userId))
                {
                    // Get the existing user first
                    User existingUser = _userService.GetUserById(userId);
                    if (existingUser == null)
                    {
                        MessageBox.Show("User not found.");
                        return;
                    }

                    // Only update role and status
                    existingUser.Role = cboRole.Text;
                    existingUser.Status = cboStatus.Text == "Active";

                    _userService.UpdateUser(existingUser);
                    MessageBox.Show("User updated successfully!");
                    LoadUserList();
                    
                    // Reset the form after update
                    ResetInput();
                    SetCreateMode();
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }
    }
}
