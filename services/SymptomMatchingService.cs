using System.Collections.Generic;
using System.Linq;

namespace DermaSmart.API.services
{
    public class SymptomMatchingService
    {
        private readonly Dictionary<string, List<string>> _mapping =
            new Dictionary<string, List<string>>
        {
            { "acne", new List<string> { "salicylic acid", "niacinamide" } },
            { "dryness", new List<string> { "hyaluronic acid", "glycerin" } }
        };

        public List<string> GetIngredientsForSymptoms(List<string> symptoms)
        {
            var result = new List<string>();

            foreach (var symptom in symptoms)
            {
                if (_mapping.ContainsKey(symptom.ToLower()))
                {
                    result.AddRange(_mapping[symptom.ToLower()]);
                }
            }

            return result.Distinct().ToList();
        }
    }
}