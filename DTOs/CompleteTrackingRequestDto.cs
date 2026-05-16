using System;
using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.DTOs
{
    public class CompleteTrackingRequestDto
    {
        [Required(ErrorMessage = "Kullanıcı ID (UserId) zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kullanıcı ID (UserId) 0'dan büyük olmalıdır.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Adım ID (StepId) zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Adım ID (StepId) 0'dan büyük olmalıdır.")]
        public int StepId { get; set; }

        [Required(ErrorMessage = "Tarih alanı zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
