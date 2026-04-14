using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RmbCoaching.Api.Data;

namespace RmbCoaching.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly RmbCoachingContext _context;

        public CoursesController(RmbCoachingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var courses = await _context.Courses
                .Where(x => x.IsActive)
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.ShortDescription,
                    x.Price,
                    x.DurationInDays,
                    x.Category,
                    x.DifficultyLevel,
                    x.ImageUrl,
                    x.IsSubscription
                })
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var course = await _context.Courses
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.ShortDescription,
                    x.Description,
                    x.Price,
                    x.DurationInDays,
                    x.Category,
                    x.DifficultyLevel,
                    x.ImageUrl,
                    x.IsSubscription,
                    x.IsActive,
                    x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (course is null)
            {
                return NotFound(new { message = "A kurzus nem található." });
            }

            return Ok(course);
        }
    }
}
