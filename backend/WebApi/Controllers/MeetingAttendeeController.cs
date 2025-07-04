using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingAttendeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MeetingAttendeeController(AppDbContext context)
        {
            _context = context;
        }

        // Invite user to meeting
        [Authorize]
        [HttpPost("invite")]
        public async Task<IActionResult> InviteUser([FromBody] InviteAttendeeDto dto)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var booking = await _context.Bookings.FindAsync(dto.BookingId);
            if (booking == null || booking.UserId != currentUserId)
                return Unauthorized(new { message = "You are not the organizer of this meeting." });

            var alreadyInvited = await _context.MeetingAttendees
                .AnyAsync(a => a.BookingId == dto.BookingId && a.UserId == dto.UserId);
            if (alreadyInvited)
                return BadRequest(new { message = "User already invited to this meeting." });

            var attendee = new MeetingAttendee
            {
                BookingId = dto.BookingId,
                UserId = dto.UserId,
                Status = "Pending"
            };

            _context.MeetingAttendees.Add(attendee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User invited successfully." });
        }

        // Get attendees of a booking
        [Authorize]
        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetAttendees(int bookingId)
        {
            var attendees = await _context.MeetingAttendees
                .Where(a => a.BookingId == bookingId)
                .Include(a => a.User)
                .Select(a => new AttendeeResponseDto
                {
                    Id = a.Id,
                    BookingId = a.BookingId,
                    UserId = a.UserId,
                    Username = a.User.Username,
                    Status = a.Status
                })
                .ToListAsync();

            return Ok(attendees);
        }

        // Invitee updates their status
        [Authorize]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateAttendeeStatusDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var attendee = await _context.MeetingAttendees
                .Include(a => a.Booking)
                .FirstOrDefaultAsync(a => a.Id == dto.AttendeeId && a.UserId == userId);

            if (attendee == null)
                return NotFound(new { message = "You are not invited to this meeting or already responded." });

            attendee.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Status updated successfully." });
        }

        [Authorize]
[HttpGet("my-invitations")]
public async Task<IActionResult> GetMyInvitations()
{
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var invitations = await _context.MeetingAttendees
        .Where(a => a.UserId == userId)
        .Include(a => a.Booking)
        .ThenInclude(b => b.Room)
        .Select(a => new
        {
            a.Id,
            a.Status,
            BookingId = a.Booking.Id,
            Room = a.Booking.Room.Name,
            Location = a.Booking.Room.Location,
            StartTime = a.Booking.StartTime,
            EndTime = a.Booking.EndTime,
            Purpose = a.Booking.Purpose,
            OrganizerUsername = a.Booking.User.Username
        })
        .ToListAsync();

    return Ok(invitations);
}

    }
}
