using System.Collections.Generic;

namespace DermaSmart.API.DTOs
{
    public class SymptomRequestDto
    {
        public List<string> Symptoms { get; set; } = new();
    }
}