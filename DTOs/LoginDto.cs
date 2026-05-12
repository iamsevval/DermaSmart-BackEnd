using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email zorunludur.")]
        [DefaultValue("ornek@domain.com")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DefaultValue("123456")]
        public string Password { get; set; } = string.Empty;
    }
}