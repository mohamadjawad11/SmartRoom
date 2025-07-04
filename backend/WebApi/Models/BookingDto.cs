namespace WebApi.Dtos
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Purpose { get; set; }
    }

    public class AttendeeDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
    }

public class BookingResponseDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; }
    public string RoomLocation { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }

    public int UserId { get; set; } // ✅ Add this line

    public List<AttendeeDto> Attendees { get; set; } = new List<AttendeeDto>();
}

}
