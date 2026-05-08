using DermaSmart.API.DTOs;

namespace DermaSmart.API.Services
{
    public class EveningRoutineService
    {
        private readonly ConflictService _conflictService;

        public EveningRoutineService(ConflictService conflictService)
        {
            _conflictService = conflictService;
        }

        public List<ProductDto> GetEveningRoutine(
            string skinType,
            List<ProductDto> products)
        {
            var eveningProducts = (products ?? new List<ProductDto>())
                .Where(p =>
                    p.IsEveningSuitable &&
                    IsEveningAllowed(p.Ingredient) &&
                    IsSuitableForSkinType(skinType, p.Type, p.Ingredient))
                .ToList();

            var safeProducts = new List<ProductDto>();

            foreach (var product in eveningProducts)
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

        private bool IsEveningAllowed(string ingredient)
        {
            ingredient = _conflictService.NormalizeIngredient(ingredient);

            return ingredient switch
            {
                "spf" => false,
                "sunscreen" => false,
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
                    type == "night cream" ||
                    ingredient == "hyaluronik asit" ||
                    ingredient == "seramid",

                "oily" or "yağlı" or "yagli" =>
                    type == "cleanser" ||
                    type == "toner" ||
                    type == "serum" ||
                    type == "treatment" ||
                    type == "moisturizer" ||
                    type == "night cream" ||
                    ingredient == "niasinamid" ||
                    ingredient == "bha" ||
                    ingredient == "benzoyl peroxide" ||
                    ingredient == "hyaluronik asit" ||
                    ingredient == "seramid",



                "combination" or "karma" =>
                    type == "cleanser" ||
                    type == "toner" ||
                    type == "serum" ||
                    type == "moisturizer" ||
                    type == "night cream" ||
                    ingredient == "niasinamid" ||
                    ingredient == "hyaluronik asit",

                "sensitive" or "hassas" =>
                    type == "cleanser" ||
                    type == "moisturizer" ||
                    type == "night cream" ||
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
                "exfoliant" => 3,
                "serum" when ingredient == "aha" || ingredient == "bha" => 3,
                "treatment" when ingredient == "retinol" => 5,
                "serum" when ingredient == "retinol" => 5,
                "serum" => 4,
                "treatment" => 6,
                "moisturizer" => 7,
                "night cream" => 8,
                _ => 99
            };
        }
    }
}
