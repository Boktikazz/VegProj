using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RmbCoaching.Api.Data;
using RmbCoaching.Api.Dtos.Orders;
using RmbCoaching.Api.Models;
using System.Security.Claims;

namespace RmbCoaching.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly RmbCoachingContext _context;

        public OrdersController(RmbCoachingContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<OrderCreateResponseDto>> CreateOrder()
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
                return BadRequest(new { message = "A kosár üres." });
            }

            var order = new Order
            {
                UserId = userId.Value,
                CourseId = cartItem.CourseId,
                PriceAtPurchase = cartItem.Course.Price,
                OrderDate = DateTime.Now,
                IsCancelled = false
            };

            _context.Orders.Add(order);
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new OrderCreateResponseDto
            {
                OrderId = order.Id,
                Message = "A rendelés sikeresen leadva. Hamarosan keresni fogunk."
            });
        }

        [HttpGet("my")]
        public async Task<ActionResult> GetMyOrders()
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Include(x => x.Course)
                .Where(x => x.UserId == userId.Value)
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new
                {
                    x.Id,
                    x.CourseId,
                    courseTitle = x.Course.Title,
                    x.PriceAtPurchase,
                    x.OrderDate,
                    x.IsCancelled,
                    x.CancelledAt,
                    x.Course.IsSubscription
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<ActionResult> CancelOrder(int id)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId.Value);
            if (order is null)
            {
                return NotFound(new { message = "A rendelés nem található." });
            }

            if (order.IsCancelled)
            {
                return BadRequest(new { message = "A rendelés már le lett mondva." });
            }

            order.IsCancelled = true;
            order.CancelledAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { message = "A rendelés lemondva." });
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}
