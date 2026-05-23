using System.ComponentModel.DataAnnotations;

namespace DermaSmart.API.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Ürün ID (Id) zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Ürün ID (Id) 0'dan büyük olmalıdır.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı (Name) zorunludur.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ürün tipi (Type) zorunludur.")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ürün içeriği (Ingredient) zorunludur.")]
        public string Ingredient { get; set; } = string.Empty;

        public bool IsMorningSuitable { get; set; }

        public bool IsEveningSuitable { get; set; }
    }
}

