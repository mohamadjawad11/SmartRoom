using System;

namespace WebApi.Models
{
    public class MeetingTask
    {
        public int Id { get; set; }

        public int BookingId { get; set; }           // Foreign Key to Booking
        public int AssignedUserId { get; set; }      // Foreign Key to User

        public string TaskDescription { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties (no data annotations required)
        public Booking? Booking { get; set; }
        public User? AssignedUser { get; set; }
    }
}
