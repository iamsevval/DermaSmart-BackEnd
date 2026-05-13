using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DermaSmart.API.DTOs
{
    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty; // ← EKLE

        [Required(ErrorMessage = "Email zorunludur.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Lütfen geçerli bir email adresi giriniz (Örn: isim@domain.com).")]
        [DefaultValue("ornek@domain.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DefaultValue("123456")]
        public string Password { get; set; } = string.Empty;
    }
}