using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.DTOs
{
    public class SymptomRequestDto
    {
        [Required(ErrorMessage = "Belirti listesi (Symptoms) zorunludur.")]
        [MinLength(1, ErrorMessage = "En az bir belirti (Symptom) girilmelidir.")]
        public List<string> Symptoms { get; set; } = new();
    }
}