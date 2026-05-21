using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DermaSmart.API.Services
{
    public class CelestiaService
    {
        private readonly Dictionary<string, List<string>> _symptomIngredientMap;

        public CelestiaService()
        {
            _symptomIngredientMap = LoadCsv("Data/CELESTIA_SKINCARE_DATASET_KAGGLE_READY.csv");
        }

        private Dictionary<string, List<string>> LoadCsv(string path)
        {
            var map = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path))
                return map;

            var lines = File.ReadAllLines(path);

            foreach (var line in lines.Skip(1))
            {
                var columns = SplitCsvLine(line);

                if (columns.Length < 7)
                    continue;

                var concern = Normalize(columns[4]);
                var ingredients = ExtractIngredients(columns[6]);

                if (string.IsNullOrWhiteSpace(concern) || ingredients.Count == 0)
                    continue;

                if (!map.ContainsKey(concern))
                    map[concern] = new List<string>();

                foreach (var ingredient in ingredients)
                {
                    if (!map[concern].Contains(ingredient))
                        map[concern].Add(ingredient);
                }
            }

            return map;
        }

        public List<string> GetIngredientsForSymptoms(List<string> symptoms)
        {
            var result = new List<string>();

            foreach (var symptom in symptoms)
            {
                var key = NormalizeSymptomAlias(symptom);

                if (string.IsNullOrWhiteSpace(key))
                    continue;

                if (_symptomIngredientMap.ContainsKey(key))
                {
                    result.AddRange(_symptomIngredientMap[key]);
                    continue;
                }

                var match = _symptomIngredientMap.Keys
                    .FirstOrDefault(k => k.Contains(key) || key.Contains(k));

                if (match != null)
                {
                    result.AddRange(_symptomIngredientMap[match]);
                }
            }

            return result.Distinct().ToList();
        }

        private string NormalizeSymptomAlias(string input)
        {
            var value = Normalize(input);

            return value switch
            {
                "sivilce" => "acne",
                "akne" => "acne",
                "leke" => "darkspots",
                "ciltlekesi" => "darkspots",
                "hiperpigmentasyon" => "hyperpigmentation",
                "kuruluk" => "dullness",
                "gozenek" => "openpores",
                "gözenek" => "openpores",
                "kizariklik" => "redness",
                "kızarıklık" => "redness",
                "siyahnokta" => "whiteheads/blackheads",
                "beyaznokta" => "whiteheads/blackheads",
                "kirisiklik" => "wrinkles",
                "kırışıklık" => "wrinkles",
                _ => value
            };
        }

        private List<string> ExtractIngredients(string value)
        {
            return (value ?? string.Empty)
                .Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(RemoveConcentration)
                .Select(i => i.ToLower().Trim())
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Distinct()
                .ToList();
        }

        private string RemoveConcentration(string value)
        {
            return Regex.Replace(value ?? string.Empty, @"\s*\d+(\.\d+)?%?", "").Trim();
        }

        private string Normalize(string input)
        {
            return (input ?? string.Empty)
                .Trim()
                .ToLower()
                .Replace(" ", "")
                .Replace("-", "");
        }

        private string[] SplitCsvLine(string line)
        {
            var values = new List<string>();
            var current = "";
            var inQuotes = false;

            foreach (var ch in line)
            {
                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (ch == ',' && !inQuotes)
                {
                    values.Add(current);
                    current = "";
                    continue;
                }

                current += ch;
            }

            values.Add(current);

            return values.ToArray();
        }
    }
}