using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.DTOs
{
    public class GenerateRoutineRequestDto
    {
        [Required(ErrorMessage = "Kullanıcı ID (UserId) zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kullanıcı ID (UserId) 0'dan büyük olmalıdır.")]
        public int UserId { get; set; }
    }
}
