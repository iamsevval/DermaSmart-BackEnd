using System.Collections.Generic;

namespace DermaSmart.API.DTOs
{
    public class MorningRoutineRequestDto
    {
        public string SkinType { get; set; } = string.Empty;

        public List<ProductDto> Products { get; set; } = new();
    }
}