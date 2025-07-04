using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
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

    }
}
