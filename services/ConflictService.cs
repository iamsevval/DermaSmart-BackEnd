namespace DermaSmart.API.Services
{
    public class ConflictService
    {
        public bool HasConflict(string firstIngredient, string secondIngredient)
        {
            var first = NormalizeIngredient(firstIngredient);
            var second = NormalizeIngredient(secondIngredient);

            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(second))
                return false;

            if (first == second)
                return true;

            return IsPair(first, second, "retinol", "vitamin c")
                || IsPair(first, second, "retinol", "aha")
                || IsPair(first, second, "retinol", "bha")
                || IsPair(first, second, "retinol", "benzoyl peroxide")
                || IsPair(first, second, "vitamin c", "aha")
                || IsPair(first, second, "vitamin c", "bha")
                || IsPair(first, second, "vitamin c", "benzoyl peroxide")
                || IsPair(first, second, "aha", "bha")
                || IsPair(first, second, "aha", "benzoyl peroxide")
                || IsPair(first, second, "bha", "benzoyl peroxide");
        }

        public List<string> GetConflicts(List<string> ingredients)
        {
            var conflicts = new List<string>();

            var normalizedIngredients = (ingredients ?? new List<string>())
                .Select(NormalizeIngredient)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Distinct()
                .ToList();

            for (int i = 0; i < normalizedIngredients.Count; i++)
            {
                for (int j = i + 1; j < normalizedIngredients.Count; j++)
                {
                    var first = normalizedIngredients[i];
                    var second = normalizedIngredients[j];

                    if (HasConflict(first, second))
                        conflicts.Add($"{first} + {second}");
                }
            }

            return conflicts;
        }

        public string NormalizeIngredient(string ingredient)
        {
            ingredient = ingredient?.ToLower().Trim() ?? "";

            return ingredient switch
            {
                "c vitamini" => "vitamin c",
                "retinoid" => "retinol",
                "retinol / retinoid" => "retinol",
                "glycolic acid" => "aha",
                "glikolik asit" => "aha",
                "lactic acid" => "aha",
                "laktik asit" => "aha",
                "salicylic acid" => "bha",
                "salisilik asit" => "bha",
                "benzoyl peroksit" => "benzoyl peroxide",
                "hyaluronic acid" => "hyaluronik asit",
                "spf 30+" => "spf",
                "gunes kremi" => "sunscreen",
                "güneş kremi" => "sunscreen",
                _ => ingredient
            };
        }

        private bool IsPair(string a, string b, string first, string second)
        {
            return (a == first && b == second) || (a == second && b == first);
        }
    }
}
