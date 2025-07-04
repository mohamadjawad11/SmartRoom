namespace WebApi.Models
{
    public class MeetingAttendee
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Status { get; set; } = "Pending"; // "Pending", "Accepted", "Declined"
    }
}
