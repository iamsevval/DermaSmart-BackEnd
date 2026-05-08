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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPost("morning-routine")]
        public IActionResult GetMorningRoutine([FromBody] MorningRoutineRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.SkinType))
                return BadRequest("SkinType boş olamaz");

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
                return BadRequest("SkinType boş olamaz");

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
        return BadRequest("Ingredients bos olamaz");

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
                return BadRequest("Symptoms boş olamaz");

            var ingredients = _celestiaService.GetIngredientsForSymptoms(request.Symptoms);

            return Ok(new
            {
                recommendedIngredients = ingredients
            });
        }
    }
}
