using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Models;
using WebApi.Dtos;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingNotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MeetingNotesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/MeetingNotes
        [Authorize]
[HttpPost]
public async Task<IActionResult> AddNote([FromBody] AddMeetingNoteRequest request)
{
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    // Optional: check if the user is part of the meeting
    var booking = await _context.Bookings
        .Include(b => b.Attendees)
        .FirstOrDefaultAsync(b => b.Id == request.BookingId);

    if (booking == null)
        return NotFound(new { message = "Meeting not found." });

    if (booking.UserId != userId &&
        !booking.Attendees.Any(a => a.UserId == userId))
        return Forbid();

    var note = new MeetingNote
    {
        BookingId = request.BookingId,
        UserId = userId,
        Content = request.Content,
        CreatedAt = DateTime.UtcNow
    };

    _context.MeetingNotes.Add(note);
    await _context.SaveChangesAsync();

    return Ok(new { message = "Note added successfully." });
}

        // GET: api/MeetingNotes/5
        [Authorize]
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetNotes(int bookingId)
        {
            var notes = await _context.MeetingNotes
                .Where(n => n.BookingId == bookingId)
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Content,
                    n.CreatedAt,
                    Username = n.User.Username
                })
                .ToListAsync();

            return Ok(notes);
        }
    }
}
