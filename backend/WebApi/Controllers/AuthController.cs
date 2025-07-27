using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Login successful",
                token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("request-reset")]
public async Task<IActionResult> RequestResetPassword([FromBody] EmailDto model, [FromServices] EmailService emailService)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
    if (user == null)
        return NotFound(new { message = "User not found" });

    // Generate 6-digit OTP
    var otp = new Random().Next(100000, 999999).ToString();

    // Save OTP to user record or a new ResetPassword table
    user.PasswordResetCode = otp;
    user.PasswordResetExpiry = DateTime.UtcNow.AddMinutes(10);
    await _context.SaveChangesAsync();

    string body = $"<p>Your password reset code is <strong>{otp}</strong>.</p>";
    await emailService.SendAsync(user.Email, "Password Reset Code", body);

    return Ok(new { message = "Reset code sent to email" });
}

public class EmailDto
{
    public string Email { get; set; }
}

[HttpPost("verify-otp")]
public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto model)
{
    var user = await _context.Users.FirstOrDefaultAsync(u =>
        u.Email == model.Email &&
        u.PasswordResetCode == model.Code &&
        u.PasswordResetExpiry > DateTime.UtcNow);

    if (user == null)
        return BadRequest(new { message = "Invalid or expired code" });

    return Ok(new { message = "OTP verified" });
}

public class OtpVerifyDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}

[HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
{
    var user = await _context.Users.FirstOrDefaultAsync(u =>
        u.Email == model.Email &&
        u.PasswordResetCode == model.Code &&
        u.PasswordResetExpiry > DateTime.UtcNow);

    if (user == null)
        return BadRequest(new { message = "Invalid or expired code" });

    user.Password = model.NewPassword;
    user.PasswordResetCode = null;
    user.PasswordResetExpiry = null;

    await _context.SaveChangesAsync();

    return Ok(new { message = "Password reset successful" });
}

public class ResetPasswordDto
{
    public string Email { get; set; }
    public string Code { get; set; }
    public string NewPassword { get; set; }
}

    }
}
