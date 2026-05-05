namespace DermaSmart.API.Models
{
    public class RoutineStep
    {
        public int Id { get; set; }
        public int StepOrder { get; set; } // Adım sırası (1, 2, 3...)
        public string TimeOfDay { get; set; } = string.Empty; // "Sabah" veya "Akşam"
        
        // Product tablosu ile ilişki (Foreign Key)
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}