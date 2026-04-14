namespace RmbCoaching.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledAt { get; set; }

        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
