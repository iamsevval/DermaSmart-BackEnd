namespace DermaSmart.API.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Ingredient { get; set; } = string.Empty;

        public bool IsMorningSuitable { get; set; }

        public bool IsEveningSuitable { get; set; }
    }
}
