using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingTaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MeetingTaskController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/MeetingTask
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AssignTask([FromBody] MeetingTask task)
        {
            // Optional: validate if user is part of the meeting before assigning
            var booking = await _context.Bookings
                .Include(b => b.Attendees)
                .FirstOrDefaultAsync(b => b.Id == task.BookingId);

            if (booking == null)
                return NotFound(new { message = "Meeting not found." });

            var assigningUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (booking.UserId != assigningUserId &&
                !booking.Attendees.Any(a => a.UserId == assigningUserId))
                return Forbid();

            task.Id = 0; // Let EF generate the ID
            _context.MeetingTasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task assigned successfully." });
        }

        // GET: api/MeetingTask/5
        [Authorize]
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetTasks(int bookingId)
        {
            var tasks = await _context.MeetingTasks
                .Where(t => t.BookingId == bookingId)
                .Include(t => t.AssignedUser)
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    t.Id,
                    t.TaskDescription,
                    t.BookingId,
                    AssignedTo = t.AssignedUser.Username
                })
                .ToListAsync();

            return Ok(tasks);
        }
    }
}
