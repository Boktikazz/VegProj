using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RmbCoaching.Api.Data;
using RmbCoaching.Api.Dtos.Courses;
using RmbCoaching.Api.Models;

namespace RmbCoaching.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/courses")]
    [ApiController]
    public class AdminCoursesController : ControllerBase
    {
        private readonly RmbCoachingContext _context;

        public AdminCoursesController(RmbCoachingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var courses = await _context.Courses
                .OrderBy(x => x.Id)
                .ToListAsync();

            return Ok(courses);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CourseCreateDto dto)
        {
            var course = new Course
            {
                Title = dto.Title.Trim(),
                ShortDescription = dto.ShortDescription.Trim(),
                Description = dto.Description.Trim(),
                Price = dto.Price,
                DurationInDays = dto.DurationInDays,
                Category = dto.Category.Trim(),
                DifficultyLevel = dto.DifficultyLevel.Trim(),
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim(),
                IsSubscription = dto.IsSubscription,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, CourseUpdateDto dto)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course is null)
            {
                return NotFound(new { message = "A kurzus nem található." });
            }

            course.Title = dto.Title.Trim();
            course.ShortDescription = dto.ShortDescription.Trim();
            course.Description = dto.Description.Trim();
            course.Price = dto.Price;
            course.DurationInDays = dto.DurationInDays;
            course.Category = dto.Category.Trim();
            course.DifficultyLevel = dto.DifficultyLevel.Trim();
            course.ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim();
            course.IsSubscription = dto.IsSubscription;
            course.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return Ok(course);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course is null)
            {
                return NotFound(new { message = "A kurzus nem található." });
            }

            course.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "A kurzus inaktiválva lett." });
        }
    }
}
