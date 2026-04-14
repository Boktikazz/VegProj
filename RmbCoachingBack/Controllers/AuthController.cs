using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RmbCoaching.Api.Data;
using RmbCoaching.Api.Dtos.Auth;
using RmbCoaching.Api.Models;
using RmbCoaching.Api.Services;
using System.Security.Claims;

namespace RmbCoaching.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RmbCoachingContext _context;
        private readonly GenerateToken _generateToken;
        private readonly PasswordService _passwordService;

        public AuthController(RmbCoachingContext context, GenerateToken generateToken, PasswordService passwordService)
        {
            _context = context;
            _generateToken = generateToken;
            _passwordService = passwordService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Minden mező kitöltése kötelező." });
            }

            var normalizedEmail = dto.Email.Trim().ToLower();
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail);
            if (existingUser is not null)
            {
                return BadRequest(new { message = "Ez az email cím már foglalt." });
            }

            var role = !await _context.Users.AnyAsync() ? "Admin" : "User";

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = normalizedEmail,
                PhoneNumber = dto.PhoneNumber.Trim(),
                PasswordHash = _passwordService.HashPassword(dto.Password),
                Role = role,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _generateToken.GenToken(user);

            return Ok(new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var normalizedEmail = dto.Email.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail);

            if (user is null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Hibás email vagy jelszó." });
            }

            var token = _generateToken.GenToken(user);

            return Ok(new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<object>> Me()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.PhoneNumber,
                    x.Role,
                    x.CreatedAt
                })
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return NotFound(new { message = "Felhasználó nem található." });
            }

            return Ok(user);
        }
    }
}
