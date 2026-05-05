namespace DermaSmart.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Temizleyici, Tonik, Serum vs.
        
        // Hangi cilt tiplerine uygun olduğu (Örn: "Yağlı, Karma" gibi metin veya JSON tutulabilir)
        public string SkinTypes { get; set; } = string.Empty; 
        
        // Ürünün barındırdığı ana içerikler
        public string Ingredients { get; set; } = string.Empty;
    }
}