using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

       [HttpGet("kpis")]
public async Task<IActionResult> GetKpis()
{
    var now = DateTime.UtcNow;
    var startOfMonth = new DateTime(now.Year, now.Month, 1);
    var endOfMonth = startOfMonth.AddMonths(1);

    var totalMeetings = await _context.Bookings
        .CountAsync(b => b.StartTime >= startOfMonth && b.StartTime < endOfMonth);

    var totalRooms = await _context.Rooms.CountAsync();

    var activeUserCount = await _context.Bookings
        .Where(b => b.StartTime >= startOfMonth && b.StartTime < endOfMonth)
        .Select(b => b.UserId)
        .Distinct()
        .CountAsync();

    var bookingsThisMonth = await _context.Bookings
        .Where(b => b.StartTime >= startOfMonth && b.StartTime < endOfMonth)
        .ToListAsync();

    double avgDuration = 0;
    double totalBookedMinutes = 0;

    if (bookingsThisMonth.Any())
    {
        var durations = bookingsThisMonth
            .Select(b => (b.EndTime - b.StartTime).TotalMinutes)
            .ToList();

        avgDuration = durations.Average();
        totalBookedMinutes = durations.Sum();
    }

    int businessDays = Enumerable.Range(0, DateTime.DaysInMonth(now.Year, now.Month))
        .Select(d => new DateTime(now.Year, now.Month, d + 1))
        .Count(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);

    double totalAvailableMinutes = totalRooms * businessDays * 8 * 60;
    var utilizationRate = totalAvailableMinutes == 0 ? 0 : (totalBookedMinutes / totalAvailableMinutes) * 100;

    return Ok(new
    {
        totalMeetings,
        totalRooms,
        activeUsers = activeUserCount,
        averageDurationMinutes = Math.Round(avgDuration, 1),
        utilizationRate = Math.Round(utilizationRate, 2)
    });
}

      [HttpGet("meetings-over-time")]
public IActionResult GetMeetingsOverTime()
{
    var now = DateTime.UtcNow;
    var startOfMonth = new DateTime(now.Year, now.Month, 1);
    var endOfMonth = startOfMonth.AddMonths(1);

    var data = _context.Bookings
        .Where(b => b.StartTime >= startOfMonth && b.StartTime < endOfMonth)
        .AsEnumerable()
        .GroupBy(b => b.StartTime.Date)
        .Select(g => new
        {
            date = g.Key.ToString("yyyy-MM-dd"),
            meetingCount = g.Count()
        })
        .OrderBy(x => x.date)
        .ToList();

    return Ok(data);
}


        [HttpGet("top-rooms")]
        public async Task<IActionResult> GetTopBookedRooms()
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var topRooms = await _context.Bookings
                .Where(b => b.StartTime >= startOfMonth && b.StartTime < endOfMonth)
                .GroupBy(b => b.RoomId)
                .Select(g => new
                {
                    RoomId = g.Key,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Join(_context.Rooms,
                      g => g.RoomId,
                      r => r.Id,
                      (g, r) => new
                      {
                          roomName = r.Name,
                          bookingCount = g.BookingCount
                      })
                .ToListAsync();

            return Ok(topRooms);
        }
        [HttpGet("top-users")]
        public async Task<IActionResult> GetTopUsers()
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var topUsers = await _context.Bookings
                .Where(b => b.StartTime >= startOfMonth && b.StartTime < endOfMonth)
                .GroupBy(b => b.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Join(_context.Users,
                      g => g.UserId,
                      u => u.Id,
                      (g, u) => new
                      {
                          username = u.Username,
                          bookingCount = g.BookingCount
                      })
                .ToListAsync();

            return Ok(topUsers);
        }

[HttpGet("hourly-distribution")]
public async Task<IActionResult> GetHourlyBookingDistribution()
{
    var now = DateTime.UtcNow;
    var startOfMonth = new DateTime(now.Year, now.Month, 1);
    var endOfMonth = startOfMonth.AddMonths(1);

    var data = await _context.Bookings
        .Where(b => b.StartTime >= startOfMonth && b.StartTime < endOfMonth)
        .GroupBy(b => b.StartTime.Hour)
        .Select(g => new
        {
            hour = g.Key,
            bookingCount = g.Count()
        })
        .OrderBy(x => x.hour)
        .ToListAsync();

    return Ok(data);
}

    }
}
