namespace RmbCoachingAdminWpf.Models;

public class CourseUpsertRequest
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public string Category { get; set; } = string.Empty;
    public string DifficultyLevel { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsSubscription { get; set; }
    public bool IsActive { get; set; } = true;
}
