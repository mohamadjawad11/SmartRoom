namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }               // Primary Key
        public string Username { get; set; }      // Used for display/login
        public string Email { get; set; }         // Unique login field
        public string Password { get; set; }      // Will be hashed later
        public string Role { get; set; }          // "Admin" or "Employee"

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
