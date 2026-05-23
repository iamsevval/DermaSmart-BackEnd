using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.DTOs
{
    public class MorningRoutineRequestDto
    {
        [Required(ErrorMessage = "Cilt tipi (SkinType) zorunludur.")]
        [MinLength(1, ErrorMessage = "Cilt tipi (SkinType) boş olamaz.")]
        public string SkinType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ürün listesi (Products) zorunludur.")]
        [MinLength(1, ErrorMessage = "En az bir ürün (Product) girilmelidir.")]
        public List<ProductDto> Products { get; set; } = new();
    }
}