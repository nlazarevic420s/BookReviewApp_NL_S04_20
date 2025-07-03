using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookReviewApp_NL_S04_20.Models;
using Microsoft.AspNetCore.Identity;
using BookReviewApp_NL_S04_20.Data;
using BookReviewApp_NL_S04_20.DTOs;

[Route("api/review")]
[ApiController]
[Authorize]
public class ReviewsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ReviewsApiController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var alreadyExists = await _context.Reviews
        .AnyAsync(r => r.BookId == dto.BookId && r.UserId == userId);
        if (alreadyExists)
        {
            return BadRequest(new { message = "You have already reviewed this book." });
        }

        var review = new Review
        {
            BookId = dto.BookId,
            Text = dto.Text,
            Rating = dto.Rating,
            UserId = userId,
            PublishedDate = DateTime.Now
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return Ok(review);
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] ReviewUpdateDto dto)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        var currentUserId = _userManager.GetUserId(User);
        if (review.UserId != currentUserId)
        {
            return Unauthorized();
        }

        if (dto.Id != id)
        {
            return BadRequest();
        }

        review.Rating = dto.Rating;
        review.Text = dto.Text;

        await _context.SaveChangesAsync();
        return Ok(review);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }
            
        var userId = _userManager.GetUserId(User);
        if (review.UserId != userId || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
