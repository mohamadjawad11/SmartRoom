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
        .Where(a => a.UserId == userId) // KEEP ALL including rejected
        .Include(a => a.Booking)
        .ThenInclude(b => b.Room)
        .Include(a => a.Booking.User)
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

[Authorize]
[HttpGet("my-meetings")]
public async Task<IActionResult> GetMeetingsICanJoin()
{
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var meetings = await _context.MeetingAttendees
        .Where(a => a.UserId == userId && a.Status == "Accepted") // 👈 Filter only Accepted
        .Include(a => a.Booking)
        .ThenInclude(b => b.Room)
        .Include(a => a.Booking.User)
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

    return Ok(meetings);
}

        /////----------------------------------------------------------////////////////////////////////

        // Start a meeting (for the creator only)
        [Authorize]
        [HttpPost("start/{bookingId}")]
        public async Task<IActionResult> StartMeeting(int bookingId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Find the booking by ID
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            // Check if the current user is the creator (organizer) of the meeting
            if (booking.UserId != currentUserId)
                return Unauthorized(new { message = "You are not the organizer of this meeting." });

            // Update the booking status to "In Progress"
            booking.Status = "In Progress";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Meeting started successfully." });
        }

      [HttpPost("join/{bookingId}")]
public async Task<IActionResult> JoinMeeting(int bookingId)
{
    var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var booking = await _context.Bookings
        .Include(b => b.Room)
        .Include(b => b.Attendees)
        .FirstOrDefaultAsync(b => b.Id == bookingId);

    if (booking == null)
        return NotFound(new { message = "Booking not found." });

    // ✅ Only allow if user is organizer or attendee
    if (booking.UserId != currentUserId)
    {
        var isAttendee = booking.Attendees.Any(a => a.UserId == currentUserId);
        if (!isAttendee)
            return Unauthorized(new { message = "You are not invited to this meeting." });
    }

    // ✅ Only allow join if current time is within meeting duration
    var now = DateTime.UtcNow;
    if (now < booking.StartTime.ToUniversalTime())
        return BadRequest(new { message = "Meeting has not started yet." });

    if (now > booking.EndTime.ToUniversalTime())
        return BadRequest(new { message = "Meeting has already ended." });

    return Ok(new
    {
        message = "You can now join the meeting.",
        meetingDetails = new
        {
            Room = booking.Room.Name,
            booking.StartTime,
            booking.EndTime,
            booking.Purpose
        }
    });
}






    }
}
