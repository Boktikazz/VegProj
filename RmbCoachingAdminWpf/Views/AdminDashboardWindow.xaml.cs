using System.Windows;
using RmbCoachingAdminWpf.Models;
using RmbCoachingAdminWpf.Services;

namespace RmbCoachingAdminWpf.Views;

public partial class AdminDashboardWindow : Window
{
    private readonly ApiClient _apiClient;
    private readonly AuthResponse _auth;

    public AdminDashboardWindow(ApiClient apiClient, AuthResponse auth)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _auth = auth;
        WelcomeTextBlock.Text = $"Belépve: {_auth.FullName} ({_auth.Email}) - {_auth.Role}";
        Loaded += async (_, _) => await LoadCoursesAsync();
    }

    private async Task LoadCoursesAsync()
    {
        try
        {
            MessageTextBlock.Text = "Kurzusok betöltése...";
            CoursesDataGrid.ItemsSource = null;
            var courses = await _apiClient.GetAdminCoursesAsync();
            CoursesDataGrid.ItemsSource = courses;
            MessageTextBlock.Text = $"Betöltve: {courses.Count} kurzus";
        }
        catch (Exception ex)
        {
            MessageTextBlock.Text = ex.Message;
            MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        await LoadCoursesAsync();
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CourseEditWindow();
        if (dialog.ShowDialog() == true)
        {
            try
            {
                await _apiClient.CreateCourseAsync(dialog.GetRequest());
                await LoadCoursesAsync();
                MessageTextBlock.Text = "Kurzus sikeresen létrehozva.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { Tag: CourseDto course })
        {
            return;
        }

        var dialog = new CourseEditWindow(course);
        if (dialog.ShowDialog() == true)
        {
            try
            {
                await _apiClient.UpdateCourseAsync(course.Id, dialog.GetRequest());
                await LoadCoursesAsync();
                MessageTextBlock.Text = "Kurzus sikeresen módosítva.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { Tag: CourseDto course })
        {
            return;
        }

        var result = MessageBox.Show(
            $"Biztosan inaktiválod ezt a kurzust?\n\n{course.Title}",
            "Megerősítés",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        try
        {
            await _apiClient.DeleteCourseAsync(course.Id);
            await LoadCoursesAsync();
            MessageTextBlock.Text = "Kurzus inaktiválva lett.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        var login = new LoginWindow();
        login.Show();
        Close();
    }
}
