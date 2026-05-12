using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.DTOs
{
    public class SkinProfileDto
    {
        [Required(ErrorMessage = "SkinType zorunludur.")]
        [MinLength(1, ErrorMessage = "SkinType boş olamaz.")]
        public string? SkinType { get; set; }

        [Required(ErrorMessage = "Concerns zorunludur.")]
        [MinLength(1, ErrorMessage = "En az bir concern girilmelidir.")]
        public List<string>? Concerns { get; set; }

        [Required(ErrorMessage = "AgeRange zorunludur.")]
        [MaxLength(50)]
        [MinLength(1, ErrorMessage = "AgeRange boş olamaz.")]
        public string? AgeRange { get; set; }
    }
}