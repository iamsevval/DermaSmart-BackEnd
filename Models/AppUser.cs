using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AppSkinProfile? SkinProfile { get; set; }
    }
}