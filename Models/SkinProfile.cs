namespace DermaSmart.API.Models
{
    public class SkinProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SkinType { get; set; } = string.Empty;
        public string Concerns { get; set; } = string.Empty; 
        public string AgeRange { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}