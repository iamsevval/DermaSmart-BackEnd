using DermaSmart.API.Data;
using DermaSmart.API.DTOs;
using DermaSmart.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DermaSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorCode = "VALIDATION_ERROR";

                if (errors.Any(e => e.Contains("email", StringComparison.OrdinalIgnoreCase)))
                    errorCode = "INVALID_EMAIL_FORMAT";
                else if (errors.Any(e => e.Contains("şifre", StringComparison.OrdinalIgnoreCase)))
                    errorCode = "INVALID_PASSWORD";

                return BadRequest(new
                {
                    success = false,
                    errorCode = errorCode,
                    message = errors.FirstOrDefault(),
                  statusCode = 400
                });
            }

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new
                {
                    success = false,
                    errorCode = "EMAIL_ALREADY_EXISTS",
                    message = "Bu email zaten kayıtlı.",
                    statusCode = 400
                });

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Kayıt başarılı.",
                userId = user.Id
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Email veya şifre hatalı." });

            var token = GenerateJwtToken(user);

            return Ok(new { token, userId = user.Id });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}