using DermaSmart.API.DTOs;

namespace DermaSmart.API.Services
{
    public class MorningRoutineService
    {
        private readonly ConflictService _conflictService;

        public MorningRoutineService(ConflictService conflictService)
        {
            _conflictService = conflictService;
        }

        public List<ProductDto> GetMorningRoutine(
            string skinType,
            List<ProductDto> products)
        {
            var morningProducts = (products ?? new List<ProductDto>())
                .Where(p =>
                    p.IsMorningSuitable &&
                    IsMorningAllowed(p.Ingredient) &&
                    IsSuitableForSkinType(skinType, p.Type, p.Ingredient))
                .ToList();

            var safeProducts = new List<ProductDto>();

            foreach (var product in morningProducts)
            {
                bool hasConflict = safeProducts.Any(existing =>
                    _conflictService.HasConflict(existing.Ingredient, product.Ingredient));

                if (!hasConflict)
                {
                    safeProducts.Add(product);
                }
            }

            return safeProducts
                .OrderBy(p => GetOrder(p.Type, p.Ingredient))
                .ToList();
        }

        private bool IsMorningAllowed(string ingredient)
        {
            ingredient = _conflictService.NormalizeIngredient(ingredient);

            return ingredient switch
            {
                "retinol" => false,
                "aha" => false,
                _ => true
            };
        }

        private bool IsSuitableForSkinType(string skinType, string type, string ingredient)
        {
            skinType = skinType?.ToLower().Trim() ?? "";
            type = type?.ToLower().Trim() ?? "";
            ingredient = _conflictService.NormalizeIngredient(ingredient);

            return skinType switch
            {
                "dry" or "kuru" =>
                    type == "cleanser" ||
                    type == "serum" ||
                    type == "moisturizer" ||
                    type == "sunscreen" ||
                    ingredient == "hyaluronik asit" ||
                    ingredient == "seramid",

                "oily" or "yağlı" or "yagli" =>
                    type == "cleanser" ||
                    type == "toner" ||
                    type == "serum" ||
                    type == "moisturizer" ||
                    type == "sunscreen" ||
                    ingredient == "niasinamid" ||
                    ingredient == "bha",

                "combination" or "karma" =>
                    type == "cleanser" ||
                    type == "toner" ||
                    type == "serum" ||
                    type == "moisturizer" ||
                    type == "sunscreen" ||
                    ingredient == "niasinamid" ||
                    ingredient == "hyaluronik asit",

                "sensitive" or "hassas" =>
                    type == "cleanser" ||
                    type == "moisturizer" ||
                    type == "sunscreen" ||
                    ingredient == "hyaluronik asit" ||
                    ingredient == "seramid" ||
                    ingredient == "niasinamid",

                _ => true
            };
        }

        private int GetOrder(string type, string ingredient)
        {
            type = type?.ToLower().Trim() ?? "";
            ingredient = _conflictService.NormalizeIngredient(ingredient);

            return type switch
            {
                "cleanser" => 1,
                "toner" => 2,
                "antioxidant" => 3,
                "serum" when ingredient == "vitamin c" => 3,
                "serum" => 4,
                "moisturizer" => 5,
                "sunscreen" => 6,
                _ when ingredient == "spf" || ingredient == "sunscreen" => 6,
                _ => 99
            };
        }
    }
}
