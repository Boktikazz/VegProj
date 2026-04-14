using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RmbCoaching.Api.Data;
using RmbCoaching.Api.Dtos.Cart;
using RmbCoaching.Api.Models;
using System.Security.Claims;

namespace RmbCoaching.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly RmbCoachingContext _context;

        public CartController(RmbCoachingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetMyCart()
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var cartItem = await _context.CartItems
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.UserId == userId.Value);

            if (cartItem is null)
            {
                return Ok(new { message = "A kosár üres.", item = (object?)null });
            }

            return Ok(new
            {
                item = new
                {
                    cartItem.Id,
                    cartItem.CourseId,
                    cartItem.AddedAt,
                    course = new
                    {
                        cartItem.Course.Id,
                        cartItem.Course.Title,
                        cartItem.Course.ShortDescription,
                        cartItem.Course.Price,
                        cartItem.Course.DurationInDays,
                        cartItem.Course.Category,
                        cartItem.Course.DifficultyLevel,
                        cartItem.Course.ImageUrl,
                        cartItem.Course.IsSubscription
                    }
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(AddToCartDto dto)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == dto.CourseId && x.IsActive);
            if (course is null)
            {
                return NotFound(new { message = "A kurzus nem található." });
            }

            var existingCartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.UserId == userId.Value);
            if (existingCartItem is null)
            {
                var cartItem = new CartItem
                {
                    UserId = userId.Value,
                    CourseId = dto.CourseId,
                    AddedAt = DateTime.Now
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                existingCartItem.CourseId = dto.CourseId;
                existingCartItem.AddedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "A kurzus bekerült a kosárba." });
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearCart()
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.UserId == userId.Value);
            if (cartItem is null)
            {
                return NotFound(new { message = "A kosár már üres." });
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "A kosár ürítve lett." });
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}
