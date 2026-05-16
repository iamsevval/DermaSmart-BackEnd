namespace DermaSmart.API.Models
{
    public class RoutineStep
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int StepOrder { get; set; }

        public string TimeOfDay { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
