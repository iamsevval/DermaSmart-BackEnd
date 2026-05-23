using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DermaSmart.API.DTOs
{
    public class ConflictRequestDto
    {
        [Required(ErrorMessage = "İçerik listesi (Ingredients) zorunludur.")]
        [MinLength(1, ErrorMessage = "En az bir içerik (Ingredient) girilmelidir.")]
        public List<string> Ingredients { get; set; } = new();
    }
}