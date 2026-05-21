using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DermaSmart.API.Data;
using DermaSmart.API.Models;
using DermaSmart.API.DTOs;
using DermaSmart.API.Services;

namespace DermaSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CelestiaService _celestiaService;
        private readonly ConflictService _conflictService;
        private readonly MorningRoutineService _morningRoutineService;
        private readonly EveningRoutineService _eveningRoutineService;

        public ProductsController(
            AppDbContext context,
            CelestiaService celestiaService,
            ConflictService conflictService,
            MorningRoutineService morningRoutineService,
            EveningRoutineService eveningRoutineService)
        {
            _context = context;
            _celestiaService = celestiaService;
            _conflictService = conflictService;
            _morningRoutineService = morningRoutineService;
            _eveningRoutineService = eveningRoutineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
          [FromQuery] string? skinType,
          [FromQuery] string? concern)
        {
            var products = await _context.Products.ToListAsync();

            if (!string.IsNullOrWhiteSpace(skinType))
            {
                products = products
                    .Where(p => IsSuitableForSkinType(p.SkinTypes, skinType))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(concern))
            {
                products = products
                    .Where(p => IsSuitableForConcern(p, concern))
                    .ToList();
            }

            return Ok(new
            {
                count = products.Count,
                products = products
            });
        }

        private bool IsSuitableForSkinType(string productSkinTypes, string requestedSkinType)
        {
            productSkinTypes = NormalizeText(productSkinTypes);
            requestedSkinType = NormalizeSkinType(requestedSkinType);

            if (string.IsNullOrWhiteSpace(requestedSkinType))
                return true;

            return productSkinTypes
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(NormalizeSkinType)
                .Contains(requestedSkinType);
        }

        private bool IsSuitableForConcern(Product product, string concern)
        {
            var normalizedConcern = NormalizeText(concern);
            var ingredients = NormalizeText(product.Ingredients);
            var category = NormalizeText(product.Category);

            return normalizedConcern switch
            {
                "sivilce" or "akne" or "acne" =>
                    ingredients.Contains("bha") ||
                    ingredients.Contains("niasinamid") ||
                    ingredients.Contains("benzoyl peroxide") ||
                    ingredients.Contains("salicylic acid") ||
                    category.Contains("treatment"),

                "leke" or "blemish" or "spot" =>
                    ingredients.Contains("vitamin c") ||
                    ingredients.Contains("niasinamid") ||
                    ingredients.Contains("aha"),

                "kuruluk" or "dryness" =>
                    ingredients.Contains("hyaluronik asit") ||
                    ingredients.Contains("seramid") ||
                    category.Contains("moisturizer") ||
                    category.Contains("night cream"),

                "hassasiyet" or "sensitive" or "kizariklik" or "kızarıklık" =>
                    ingredients.Contains("seramid") ||
                    ingredients.Contains("hyaluronik asit") ||
                    ingredients.Contains("niasinamid") ||
                    category.Contains("cleanser") ||
                    category.Contains("moisturizer"),

                _ => true
            };
        }

        private string NormalizeSkinType(string value)
        {
            value = NormalizeText(value);

            return value switch
            {
                "kuru" => "dry",
                "yagli" => "oily",
                "yağlı" => "oily",
                "karma" => "combination",
                "hassas" => "sensitive",
                "normal" => "normal",
                _ => value
            };
        }

        private string NormalizeText(string value)
        {
            return value?.ToLower().Trim() ?? string.Empty;
        }
        [HttpPost("morning-routine")]
        public IActionResult GetMorningRoutine([FromBody] MorningRoutineRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.SkinType))
                return BadRequest(new { message = "SkinType boş olamaz" });

            var result = _morningRoutineService.GetMorningRoutine(
                request.SkinType,
                request.Products
            );

            return Ok(new
            {
                routine = result
            });
        }

        [HttpPost("evening-routine")]
        public IActionResult GetEveningRoutine([FromBody] MorningRoutineRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.SkinType))
                return BadRequest(new { message = "SkinType boş olamaz" });

            var result = _eveningRoutineService.GetEveningRoutine(
                request.SkinType,
                request.Products
            );

            return Ok(new
            {
                routine = result
            });
        }

        [HttpPost("check-conflicts")]
        public IActionResult CheckConflicts([FromBody] ConflictRequestDto request)
        {
            if (request?.Ingredients == null || request.Ingredients.Count == 0)
                return BadRequest(new { message = "Ingredients boş olamaz" });

            var conflicts = _conflictService.GetConflicts(request.Ingredients);

            return Ok(new
            {
                hasConflict = conflicts.Count > 0,
                conflicts = conflicts
            });
        }


        [HttpPost("match-symptoms")]
        public IActionResult MatchSymptoms([FromBody] SymptomRequestDto request)
        {
            if (request?.Symptoms == null || request.Symptoms.Count == 0)
                return BadRequest(new { message = "Symptoms boş olamaz" });

            var ingredients = _celestiaService.GetIngredientsForSymptoms(request.Symptoms);

            return Ok(new
            {
                recommendedIngredients = ingredients
            });
        }
    }
}
