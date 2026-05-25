using DermaSmart.API.Data;
using DermaSmart.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DermaSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserIdFromToken()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdStr, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Geçersiz kullanıcı token'ı.");
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var favorites = await _context.FavoriteProducts
                    .Include(f => f.Product)
                    .Where(f => f.UserId == userId)
                    .Select(f => f.Product)
                    .ToListAsync();

                return Ok(new { success = true, favorites });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddFavorite(int productId)
        {
            try
            {
                var userId = GetUserIdFromToken();

                // Ürün veritabanında var mı kontrolü
                var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
                if (!productExists)
                {
                    return NotFound(new { success = false, message = "Ürün bulunamadı." });
                }

                // Zaten favorilerde var mı kontrolü
                var existingFavorite = await _context.FavoriteProducts
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

                if (existingFavorite != null)
                {
                    return BadRequest(new { success = false, message = "Ürün zaten favorilerde." });
                }

                var favorite = new FavoriteProduct
                {
                    UserId = userId,
                    ProductId = productId
                };

                _context.FavoriteProducts.Add(favorite);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Ürün favorilere eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFavorite(int productId)
        {
            try
            {
                var userId = GetUserIdFromToken();

                var favorite = await _context.FavoriteProducts
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

                if (favorite == null)
                {
                    return NotFound(new { success = false, message = "Ürün favorilerde bulunamadı." });
                }

                _context.FavoriteProducts.Remove(favorite);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Ürün favorilerden çıkarıldı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
