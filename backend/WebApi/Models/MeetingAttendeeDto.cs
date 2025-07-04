namespace WebApi.Dtos
{
    public class InviteAttendeeDto
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
    }

    public class AttendeeResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
    }

    public class UpdateAttendeeStatusDto
    {
        public int AttendeeId { get; set; }
        public string Status { get; set; } // e.g. "Accepted", "Declined"
    }
}
