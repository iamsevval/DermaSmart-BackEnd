namespace DermaSmart.API.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        
        // Hangi içeriklerle beraber kullanılamayacağı (Örn: "AHA, BHA, Retinol" şeklinde string olarak tutulabilir)
        public string Conflicts { get; set; } = string.Empty; 
    }
}