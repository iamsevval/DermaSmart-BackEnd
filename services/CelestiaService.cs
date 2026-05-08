using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DermaSmart.API.Services
{
    public class CelestiaService
    {
        private readonly Dictionary<string, List<string>> _symptomIngredientMap;

        public CelestiaService()
        {
            _symptomIngredientMap = LoadCsv("Data/CELESTIA_SKINCARE_DATASET_KAGGLE_READY.csv");

            // DEBUG (ilk çalıştırmada hangi key'ler var görmek için)
            Console.WriteLine("Loaded symptoms:");
            Console.WriteLine(string.Join(", ", _symptomIngredientMap.Keys));
        }

        // 📌 CSV LOAD
        private Dictionary<string, List<string>> LoadCsv(string path)
        {
            var map = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path))
                return map;

            var lines = File.ReadAllLines(path);

            foreach (var line in lines.Skip(1))
            {
                var columns = line.Split(',');

                if (columns.Length < 2)
                    continue;

                var symptom = Normalize(columns[0]);
                var ingredient = Normalize(columns[1]);

                if (!map.ContainsKey(symptom))
                    map[symptom] = new List<string>();

                if (!map[symptom].Contains(ingredient))
                    map[symptom].Add(ingredient);
            }

            return map;
        }

        // 📌 PUBLIC METHOD (CONTROLLER BURAYI ÇAĞIRIYOR)
        public List<string> GetIngredientsForSymptoms(List<string> symptoms)
        {
            var result = new List<string>();

            foreach (var symptom in symptoms)
            {
                var key = Normalize(symptom);

                // 🔥 EXACT MATCH
                if (_symptomIngredientMap.ContainsKey(key))
                {
                    result.AddRange(_symptomIngredientMap[key]);
                    continue;
                }

                // 🔥 FUZZY MATCH (CSV farklı yazıldıysa kurtarır)
                var match = _symptomIngredientMap.Keys
                    .FirstOrDefault(k => k.Contains(key) || key.Contains(k));

                if (match != null)
                {
                    result.AddRange(_symptomIngredientMap[match]);
                }
            }

            return result.Distinct().ToList();
        }

        // 📌 NORMALIZER (EN KRİTİK FIX)
        private string Normalize(string input)
        {
            return input
                .Trim()
                .ToLower()
                .Replace(" ", "")
                .Replace("-", "");
        }
    }
}