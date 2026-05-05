using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.Models
{
    public class AppSkinProfile
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public string SkinType { get; set; } = string.Empty;
        public string Concerns { get; set; } = string.Empty;
        public string AgeRange { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AppUser? User { get; set; }
    }
}