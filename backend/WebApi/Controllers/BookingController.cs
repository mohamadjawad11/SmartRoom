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
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized(new { message = "Invalid token." });

            var userId = int.Parse(userIdClaim);

            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null || !room.IsAvailable)
                return BadRequest(new { message = "Room is not available or does not exist." });

            // Check for conflicts
            var overlappingBooking = await _context.Bookings
                .AnyAsync(b => b.RoomId == dto.RoomId &&
                               b.StartTime < dto.EndTime &&
                               dto.StartTime < b.EndTime);

            if (overlappingBooking)
                return Conflict(new { message = "Room is already booked in this time range." });

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                UserId = userId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Purpose = dto.Purpose,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking created successfully.", bookingId = booking.Id });
        }

        [Authorize]
        [HttpGet]

        [HttpGet]
        public async Task<IActionResult> GetUserBookings()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Room)
                .Include(b => b.Attendees)
                    .ThenInclude(ma => ma.User)
               .Select(b => new BookingResponseDto
               {
                   Id = b.Id,
                   RoomId = b.RoomId,
                   RoomName = b.Room.Name,
                   RoomLocation = b.Room.Location,
                   StartTime = b.StartTime,
                   EndTime = b.EndTime,
                   Purpose = b.Purpose,
                   Status = b.Status,

                   UserId = b.UserId, // ✅ Include this line

                   Attendees = b.Attendees.Select(a => new AttendeeDto
                   {
                       UserId = a.UserId,
                       Username = a.User.Username,
                       Status = a.Status
                   }).ToList()
               }).ToListAsync();

            return Ok(bookings);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Select(b => new
                {
                    b.Id,
                    Room = b.Room.Name,
                    User = b.User.Username,
                    b.StartTime,
                    b.EndTime,
                    b.Purpose,
                    b.Status
                })
                .ToListAsync();

            return Ok(bookings);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            // Only the owner or Admin can delete
            if (booking.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking deleted successfully." });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] CreateBookingDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            // Only the owner can update (or Admin)
            if (booking.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            // Check for conflicts (exclude current booking)
            var hasConflict = await _context.Bookings
                .AnyAsync(b => b.Id != id &&
                               b.RoomId == dto.RoomId &&
                               b.StartTime < dto.EndTime &&
                               dto.StartTime < b.EndTime);
            if (hasConflict)
                return Conflict(new { message = "Room is already booked in this time range." });

            booking.RoomId = dto.RoomId;
            booking.StartTime = dto.StartTime;
            booking.EndTime = dto.EndTime;
            booking.Purpose = dto.Purpose;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking updated successfully." });
        }

 [Authorize]
        [HttpGet("my-created-meetings")]
        public async Task<IActionResult> GetMyCreatedMeetings()
        {
            Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            // Fetch bookings where the current user is the creator (userId matches)
            var createdMeetings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Room)
                .Include(b => b.Attendees)
                .Select(b => new
                {
                    b.Id,
                    b.Room.Name,
                    b.Room.Location,
                    b.StartTime,
                    b.EndTime,
                    b.Purpose,
                    b.Status,
                    Attendees = b.Attendees.Select(a => new
                    {
                        a.UserId,
                        a.User.Username,
                        a.Status
                    }).ToList()
                })
                .ToListAsync();

            // Log the data to verify if meetings are fetched correctly
            Console.WriteLine($"Fetched {createdMeetings.Count} created meetings for user {userId}");

            if (createdMeetings.Count == 0)
            {
                return Ok(new { message = "No meetings found." });
            }

            return Ok(createdMeetings);
        }

    }
}
