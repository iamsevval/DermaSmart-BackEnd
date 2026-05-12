using DermaSmart.API.Data;
using DermaSmart.API.DTOs;
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
    public class SkinProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SkinProfileController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/skinprofile
        [HttpPost]
        public async Task<IActionResult> CreateSkinProfile([FromBody] SkinProfileDto dto)
        {
            // EMPTY BODY TEST
            if (dto == null)
            {
                return BadRequest(new
                {
                    message = "Request body boş olamaz."
                });
            }

            // REQUIRED FIELD TEST
         if (!ModelState.IsValid)
        return BadRequest(new { message = "Zorunlu alanlar eksik veya hatalı." });


            // CONCERNS REQUIRED TEST
            if (dto.Concerns == null || dto.Concerns.Count == 0 || dto.Concerns.All(string.IsNullOrWhiteSpace))
                return BadRequest(new { message = "En az bir cilt endişesi (concern) girmek zorunludur." });

            // LONG INPUT TEST
         if (dto.Concerns.Any(c => c.Length > 100))
        return BadRequest(new { message = "Concern değeri çok uzun." });

            // VALID SKIN TYPE TEST
        var validSkinTypes = new List<string> { "dry", "oily", "combination", "normal", "sensitive", "kuru", "yağlı", "yagli", "karma", "hassas" };
    if (!validSkinTypes.Contains(dto.SkinType!.Trim().ToLower()))
        return BadRequest(new { message = "Geçersiz skin type. Lütfen geçerli bir cilt tipi giriniz." });


            // CONFLICT LOGIC TEST
         var skinTypeParts = dto.SkinType.ToLower().Split(',').Select(s => s.Trim()).ToList();
    if ((skinTypeParts.Contains("oily") || skinTypeParts.Contains("yağlı")) && (skinTypeParts.Contains("dry") || skinTypeParts.Contains("kuru")))
        return BadRequest(new { message = "Birbiriyle çakışan cilt tipleri gönderilemez." });

            // VALID AGE RANGE TEST
        var validAgeRanges = new List<string> { "18-24", "25-34", "35-44", "45 ve üzeri" };
    if (string.IsNullOrEmpty(dto.AgeRange) || !validAgeRanges.Contains(dto.AgeRange))
        return BadRequest(new { message = "Geçersiz yaş aralığı. Kabul edilen değerler: 18-24, 25-34, 35-44, 45 ve üzeri." });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Token geçersiz."
                });
            }

            var userId = int.Parse(userIdClaim.Value);

            // DUPLICATE PROFILE TEST
            var existing = await _context.SkinProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (existing != null)
            {
                return BadRequest(new
                {
                    message = "Bu kullanıcı için zaten bir profil mevcut."
                });
            }

            var profile = new SkinProfile
            {
                UserId = userId,
                SkinType = dto.SkinType,

                // 🔥 DÜZELTİLEN KISIM
                Concerns = string.Join(",", dto.Concerns),

                AgeRange = dto.AgeRange!
            };

            _context.SkinProfiles.Add(profile);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cilt profili oluşturuldu.",
                profileId = profile.Id
            });
        }

        // GET: api/skinprofile/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSkinProfile(int userId)
        {
            var tokenUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (tokenUserIdStr == null || int.Parse(tokenUserIdStr) != userId)
            {
                return Unauthorized(new { message = "Bu profili görüntüleme yetkiniz yok." });
            }

            var profile = await _context.SkinProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Profil bulunamadı."
                });
            }

            return Ok(profile);
        }
    }
}