using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

     [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto credentials)
{
    if (credentials == null || string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
    {
        return BadRequest(new { message = "Email and password are required." });
    }

    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == credentials.Email && u.Password == credentials.Password);

    if (user == null)
        return Unauthorized(new { message = "Invalid email or password" });

    return Ok(new
    {
        message = "Login successful",
        user = new
        {
            user.Id,
            user.Username,
            user.Email,
            user.Role
        }
    });
}

    }
}
