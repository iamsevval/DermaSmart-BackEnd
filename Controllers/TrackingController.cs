using DermaSmart.API.Data;
using DermaSmart.API.DTOs;
using DermaSmart.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace DermaSmart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrackingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrackingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteStep([FromBody] CompleteTrackingRequestDto request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request." });
            }

            var authenticatedUserIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserIdStr == null || int.Parse(authenticatedUserIdStr) != request.UserId)
            {
                return Unauthorized(new { message = "Bu işlemi yapmaya yetkiniz yok." });
            }

            var dateOnly = request.Date.Date;

            // Kullanıcı var mı kontrolü
            var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
            {
                return NotFound(new { message = "Belirtilen kullanıcı bulunamadı." });
            }

            // Check if already completed today
            var existingLog = await _context.TrackingLogs
                .FirstOrDefaultAsync(t => t.UserId == request.UserId 
                                       && t.RoutineStepId == request.StepId 
                                       && t.CompletedDate.Date == dateOnly);

            if (existingLog != null)
            {
                // Zaten yapılmışsa hata dönmeyecek, 200 OK dönecek
                return Ok(new { message = "Step already completed for this date." });
            }

            var log = new TrackingLog
            {
                UserId = request.UserId,
                RoutineStepId = request.StepId,
                CompletedDate = request.Date,
                IsCompleted = true
            };

            _context.TrackingLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Step completed successfully." });
        }

        [HttpGet("{userId}/history")]
        public async Task<IActionResult> GetHistory(int userId)
        {
            var authenticatedUserIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserIdStr == null || int.Parse(authenticatedUserIdStr) != userId)
            {
                return Unauthorized(new { message = "Bu işlemi yapmaya yetkiniz yok." });
            }

            // Kullanıcı var mı kontrolü
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound(new { message = "Belirtilen kullanıcı bulunamadı." });
            }
            var logs = await _context.TrackingLogs
                .Where(t => t.UserId == userId && t.IsCompleted)
                .OrderByDescending(t => t.CompletedDate)
                .ToListAsync();

            var history = logs
                .GroupBy(t => t.CompletedDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    CompletedSteps = g.Select(s => new 
                    {
                        StepId = s.RoutineStepId,
                        CompletedTime = s.CompletedDate
                    }).ToList()
                })
                .ToList();

            return Ok(history);
        }
    }
}
