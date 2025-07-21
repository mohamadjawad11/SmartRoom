using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public UserController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ✅ GET /api/user/profile
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found." });

            var profileDto = new UserProfileDto
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(profileDto);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username
                })
                .ToListAsync();

            return Ok(users);
        }


        // ✅ PUT /api/user/update-password
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found." });

            if (user.Password != dto.CurrentPassword)
                return BadRequest(new { message = "Current password is incorrect." });

            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest(new { message = "New passwords do not match." });

            user.Password = dto.NewPassword; // 🔒 (Hashing can be added later)
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password updated successfully." });
        }

        [Authorize]
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return NotFound();

            return Ok(new { id = user.Id, username = user.Username });
        }

        // [Authorize(Roles = "Admin")]
        // [HttpPost]
        // public async Task<IActionResult> CreateUser([FromBody] RegisterDto dto)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
        //         return BadRequest(new { message = "Username already exists." });

        //     var user = new User
        //     {
        //         Username = dto.Username,
        //         Email = dto.Email,
        //         Password = dto.Password, // ❗ Consider hashing in real-world apps
        //         Role = dto.Role
        //     };

        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();

        //     return Ok(new { message = "Employee created successfully", userId = user.Id });
        // }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username already exists." });

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password, // ❗ Consider hashing in real-world apps
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendEmailToAllUsers(
                    $"New User Created: {user.Username}",
                    $"A new <strong>{user.Role}</strong> has been created with the username: <strong>{user.Username}</strong>."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send create-user email: " + ex.Message);
            }

            return Ok(new { message = "Employee created successfully", userId = user.Id });
        }


        //         [Authorize(Roles = "Admin")]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterDto dto)
        // {
        //     var user = await _context.Users.FindAsync(id);
        //     if (user == null)
        //         return NotFound(new { message = "User not found." });

        //     user.Username = dto.Username;
        //     user.Email = dto.Email;
        //     user.Password = dto.Password; // ❗ Again, hash if needed
        //     user.Role = dto.Role;

        //     await _context.SaveChangesAsync();
        //     return Ok(new { message = "Employee updated successfully" });
        // }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Password = dto.Password; // ❗ Again, hash if needed
            user.Role = dto.Role;

            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendEmailToAllUsers(
                    $"User Updated: {user.Username}",
                    $"User <strong>{user.Username}</strong> has been updated.<br/>" +
                    $"<strong>Role:</strong> {user.Role}<br/>" +
                    $"<strong>Email:</strong> {user.Email}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send update-user email: " + ex.Message);
            }

            return Ok(new { message = "Employee updated successfully" });
        }

        //         [Authorize(Roles = "Admin")]
        // [HttpDelete("by-username/{username}")]
        // public async Task<IActionResult> DeleteUserByUsername(string username)
        // {
        //     var user = await _context.Users
        //         .Include(u => u.Bookings)
        //         .FirstOrDefaultAsync(u => u.Username == username);

        //     if (user == null)
        //         return NotFound(new { message = "User not found." });

        //     // Remove references from MeetingAttendees
        //     var attendees = _context.MeetingAttendees.Where(ma => ma.UserId == user.Id);
        //     _context.MeetingAttendees.RemoveRange(attendees);

        //     _context.Users.Remove(user);
        //     await _context.SaveChangesAsync();

        //     return Ok(new { message = "Employee deleted successfully" });
        // }

[Authorize(Roles = "Admin")]
[HttpDelete("by-username/{username}")]
public async Task<IActionResult> DeleteUserByUsername(string username)
{
    var user = await _context.Users
        .Include(u => u.Bookings)
        .FirstOrDefaultAsync(u => u.Username == username);

    if (user == null)
        return NotFound(new { message = "User not found." });

    var attendees = _context.MeetingAttendees.Where(ma => ma.UserId == user.Id);
    _context.MeetingAttendees.RemoveRange(attendees);

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    try
    {
        await _emailService.SendEmailToAllUsers(
            $"User Deleted: {user.Username}",
            $"The user <strong>{user.Username}</strong> has been removed from the system."
        );
    }
    catch (Exception ex)
    {
        Console.WriteLine("Failed to send delete-user email: " + ex.Message);
    }

    return Ok(new { message = "Employee deleted successfully" });
}


    }
}

