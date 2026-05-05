using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // İlişkiler (Navigation Properties)
        public SkinProfile? SkinProfile { get; set; }
    }
}