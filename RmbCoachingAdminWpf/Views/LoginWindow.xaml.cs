using System.Windows;
using RmbCoachingAdminWpf.Models;
using RmbCoachingAdminWpf.Services;

namespace RmbCoachingAdminWpf.Views;

public partial class LoginWindow : Window
{
    private readonly ApiClient _apiClient = new();

    public LoginWindow()
    {
        InitializeComponent();
        EmailTextBox.Text = "admin@rmb.hu";
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ToggleUi(false);
            StatusTextBlock.Text = string.Empty;

            var auth = await _apiClient.LoginAsync(new LoginRequest
            {
                Email = EmailTextBox.Text.Trim(),
                Password = PasswordBox.Password
            });

            if (!string.Equals(auth.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                StatusTextBlock.Text = "Ez a felhasználó nem admin szerepkörű.";
                return;
            }

            _apiClient.SetToken(auth.Token);

            var dashboard = new AdminDashboardWindow(_apiClient, auth);
            dashboard.Show();
            Close();
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = ex.Message;
        }
        finally
        {
            ToggleUi(true);
        }
    }

    private void ToggleUi(bool isEnabled)
    {
        LoginButton.IsEnabled = isEnabled;
        EmailTextBox.IsEnabled = isEnabled;
        PasswordBox.IsEnabled = isEnabled;
    }
}
