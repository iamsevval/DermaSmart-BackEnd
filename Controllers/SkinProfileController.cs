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
        public async Task<IActionResult> CreateSkinProfile(SkinProfileDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Token geçersiz." });

            var userId = int.Parse(userIdClaim.Value);

            var existing = await _context.SkinProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (existing != null)
                return BadRequest(new { message = "Bu kullanıcı için zaten bir profil mevcut." });

            var profile = new SkinProfile
            {
                UserId = userId,
                SkinType = dto.SkinType,
                Concerns = dto.Concerns,
                AgeRange = dto.AgeRange
            };

            _context.SkinProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cilt profili oluşturuldu.", profileId = profile.Id });
        }

        // GET: api/skinprofile/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSkinProfile(int userId)
        {
            var profile = await _context.SkinProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (profile == null)
                return NotFound(new { message = "Profil bulunamadı." });

            return Ok(profile);
        }
    }
}