using DermaSmart.API.Data;
using DermaSmart.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DermaSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var authenticatedUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserIdStr == null)
            {
                return Unauthorized(new { message = "Yetkisiz erişim." });
            }

            var userId = int.Parse(authenticatedUserIdStr);
            // Return only the current user for safety, preventing PII leak of other users
            return await _context.Users
                .Where(u => u.Id == userId)
                .ToListAsync();
        }

        // GET: api/Users/me
        [HttpGet("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var authenticatedUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserIdStr == null)
            {
                return Unauthorized(new { message = "Yetkisiz erişim." });
            }

            var userId = int.Parse(authenticatedUserIdStr);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Eğer test için POST atılıyorsa şifrenin hashli kaydedildiğinden emin olalım
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }
    }
}