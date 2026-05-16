using DermaSmart.API.Data;
using DermaSmart.API.DTOs;
using DermaSmart.API.Models;
using DermaSmart.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DermaSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutineController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MorningRoutineService _morningRoutineService;
        private readonly EveningRoutineService _eveningRoutineService;
        private readonly CelestiaService _celestiaService;

        public RoutineController(
            AppDbContext context,
            MorningRoutineService morningRoutineService,
            EveningRoutineService eveningRoutineService,
            CelestiaService celestiaService)
        {
            _context = context;
            _morningRoutineService = morningRoutineService;
            _eveningRoutineService = eveningRoutineService;
            _celestiaService = celestiaService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateRoutine([FromBody] GenerateRoutineRequestDto request)
        {
            if (request == null || request.UserId <= 0)
                return BadRequest(new { message = "Gecerli bir userId gonderilmelidir." });

            var skinProfile = await _context.SkinProfiles
                .FirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (skinProfile == null)
                return NotFound(new { message = "Kullaniciya ait cilt profili bulunamadi." });

            var products = await _context.Products.ToListAsync();

            if (products.Count == 0)
                return NotFound(new { message = "Rutin olusturmak icin urun bulunamadi." });

            var recommendedIngredients = GetRecommendedIngredients(skinProfile.Concerns);

            var productDtos = products
                .Select(p => ToProductDto(p, recommendedIngredients))
                .ToList();

            var morningRoutine = _morningRoutineService.GetMorningRoutine(
                skinProfile.SkinType,
                productDtos);

            var eveningRoutine = _eveningRoutineService.GetEveningRoutine(
                skinProfile.SkinType,
                productDtos);

            var oldSteps = await _context.RoutineSteps
                .Where(r => r.UserId == request.UserId)
                .ToListAsync();

            _context.RoutineSteps.RemoveRange(oldSteps);

            AddRoutineSteps(request.UserId, "morning", morningRoutine);
            AddRoutineSteps(request.UserId, "evening", eveningRoutine);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                userId = request.UserId,
                skinType = skinProfile.SkinType,
                concerns = SplitConcerns(skinProfile.Concerns),
                morningRoutine,
                eveningRoutine
            });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRoutine(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Gecerli bir userId gonderilmelidir." });

            var skinProfile = await _context.SkinProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (skinProfile == null)
                return NotFound(new { message = "Kullaniciya ait cilt profili bulunamadi." });

            var routineSteps = await _context.RoutineSteps
                .Include(r => r.Product)
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.TimeOfDay)
                .ThenBy(r => r.StepOrder)
                .ToListAsync();

            if (routineSteps.Count == 0)
                return NotFound(new { message = "Bu kullanici icin generate edilmis rutin bulunamadi." });

            return Ok(new
            {
                userId,
                skinType = skinProfile.SkinType,
                concerns = SplitConcerns(skinProfile.Concerns),
                morningRoutine = ToRoutineResponse(routineSteps, "morning"),
                eveningRoutine = ToRoutineResponse(routineSteps, "evening")
            });
        }

        private List<string> GetRecommendedIngredients(string concerns)
        {
            var concernList = SplitConcerns(concerns);

            if (concernList.Count == 0)
                return new List<string>();

            return _celestiaService.GetIngredientsForSymptoms(concernList);
        }

        private ProductDto ToProductDto(Product product, List<string> recommendedIngredients)
        {
            var ingredient = GetPrimaryIngredient(product.Ingredients);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Type = NormalizeProductType(product.Category),
                Ingredient = ingredient,
                IsMorningSuitable = IsMorningSuitable(product.Category, ingredient, recommendedIngredients),
                IsEveningSuitable = IsEveningSuitable(product.Category, ingredient, recommendedIngredients)
            };
        }

        private void AddRoutineSteps(int userId, string timeOfDay, List<ProductDto> routine)
        {
            for (int i = 0; i < routine.Count; i++)
            {
                if (routine[i].Id <= 0)
                    continue;

                _context.RoutineSteps.Add(new RoutineStep
                {
                    UserId = userId,
                    ProductId = routine[i].Id,
                    TimeOfDay = timeOfDay,
                    StepOrder = i + 1,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        private List<object> ToRoutineResponse(List<RoutineStep> routineSteps, string timeOfDay)
        {
            return routineSteps
                .Where(r => r.TimeOfDay == timeOfDay)
                .OrderBy(r => r.StepOrder)
                .Select(r => new
                {
                    stepOrder = r.StepOrder,
                    timeOfDay = r.TimeOfDay,
                    productId = r.ProductId,
                    productName = r.Product?.Name,
                    type = r.Product?.Category,
                    ingredient = r.Product?.Ingredients
                })
                .Cast<object>()
                .ToList();
        }

        private List<string> SplitConcerns(string concerns)
        {
            return (concerns ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }

        private string GetPrimaryIngredient(string ingredients)
        {
            return (ingredients ?? string.Empty)
                .Split(new[] { ',', ';', '/', '|' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault() ?? string.Empty;
        }

        private string NormalizeProductType(string category)
        {
            category = category?.ToLower().Trim() ?? "";

            return category switch
            {
                "temizleyici" => "cleanser",
                "tonik" => "toner",
                "serum" => "serum",
                "nemlendirici" => "moisturizer",
                "gunes kremi" => "sunscreen",
                "güneş kremi" => "sunscreen",
                "gece kremi" => "night cream",
                "peeling" => "exfoliant",
                "tedavi" => "treatment",
                _ => category
            };
        }

        private bool IsMorningSuitable(string category, string ingredient, List<string> recommendedIngredients)
        {
            var type = NormalizeProductType(category);
            ingredient = ingredient?.ToLower().Trim() ?? "";

            if (type == "sunscreen")
                return true;

            if (ingredient == "retinol")
                return false;

            return recommendedIngredients.Count == 0
                || recommendedIngredients.Any(i => ingredient.Contains(i, StringComparison.OrdinalIgnoreCase))
                || IsBasicRoutineType(type);
        }

        private bool IsEveningSuitable(string category, string ingredient, List<string> recommendedIngredients)
        {
            var type = NormalizeProductType(category);
            ingredient = ingredient?.ToLower().Trim() ?? "";

            if (type == "sunscreen" || ingredient == "spf")
                return false;

            return recommendedIngredients.Count == 0
                || recommendedIngredients.Any(i => ingredient.Contains(i, StringComparison.OrdinalIgnoreCase))
                || IsBasicRoutineType(type)
                || type == "night cream"
                || type == "treatment"
                || type == "exfoliant";
        }

        private bool IsBasicRoutineType(string type)
        {
            return type == "cleanser"
                || type == "toner"
                || type == "serum"
                || type == "moisturizer";
        }
    }
}
