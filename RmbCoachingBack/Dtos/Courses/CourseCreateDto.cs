namespace RmbCoaching.Api.Dtos.Courses
{
    public class CourseCreateDto
    {
        public string Title { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string Category { get; set; } = null!;
        public string DifficultyLevel { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsSubscription { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
