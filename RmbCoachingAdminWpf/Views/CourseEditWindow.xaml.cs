using System.Globalization;
using System.Windows;
using RmbCoachingAdminWpf.Models;

namespace RmbCoachingAdminWpf.Views;

public partial class CourseEditWindow : Window
{
    private readonly CourseDto? _course;
    private CourseUpsertRequest? _request;

    public CourseEditWindow()
    {
        InitializeComponent();
        HeaderTextBlock.Text = "Új kurzus";
    }

    public CourseEditWindow(CourseDto course) : this()
    {
        _course = course;
        HeaderTextBlock.Text = "Kurzus szerkesztése";

        TitleTextBox.Text = course.Title;
        ShortDescriptionTextBox.Text = course.ShortDescription;
        DescriptionTextBox.Text = course.Description;
        PriceTextBox.Text = course.Price.ToString(CultureInfo.InvariantCulture);
        DurationTextBox.Text = course.DurationInDays.ToString();
        CategoryTextBox.Text = course.Category;
        DifficultyTextBox.Text = course.DifficultyLevel;
        ImageUrlTextBox.Text = course.ImageUrl ?? string.Empty;
        IsSubscriptionCheckBox.IsChecked = course.IsSubscription;
        IsActiveCheckBox.IsChecked = course.IsActive;
    }

    public CourseUpsertRequest GetRequest()
    {
        return _request ?? throw new InvalidOperationException("Nincs mentett adat.");
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        ValidationTextBlock.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
            string.IsNullOrWhiteSpace(ShortDescriptionTextBox.Text) ||
            string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
            string.IsNullOrWhiteSpace(CategoryTextBox.Text) ||
            string.IsNullOrWhiteSpace(DifficultyTextBox.Text))
        {
            ValidationTextBlock.Text = "Minden kötelező mezőt tölts ki.";
            return;
        }

        if (!decimal.TryParse(PriceTextBox.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
        {
            ValidationTextBlock.Text = "Az ár nem megfelelő formátumú.";
            return;
        }

        if (!int.TryParse(DurationTextBox.Text, out var duration))
        {
            ValidationTextBlock.Text = "A napok száma egész szám legyen.";
            return;
        }

        _request = new CourseUpsertRequest
        {
            Title = TitleTextBox.Text.Trim(),
            ShortDescription = ShortDescriptionTextBox.Text.Trim(),
            Description = DescriptionTextBox.Text.Trim(),
            Price = price,
            DurationInDays = duration,
            Category = CategoryTextBox.Text.Trim(),
            DifficultyLevel = DifficultyTextBox.Text.Trim(),
            ImageUrl = string.IsNullOrWhiteSpace(ImageUrlTextBox.Text) ? null : ImageUrlTextBox.Text.Trim(),
            IsSubscription = IsSubscriptionCheckBox.IsChecked == true,
            IsActive = IsActiveCheckBox.IsChecked == true
        };

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
