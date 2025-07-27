namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }               // Primary Key
        public string Username { get; set; }      // Used for display/login
        public string Email { get; set; }         // Unique login field
        public string Password { get; set; }      // Will be hashed later
        public string Role { get; set; }          // "Admin" or "Employee"

        // 🔐 New fields for reset password OTP
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
