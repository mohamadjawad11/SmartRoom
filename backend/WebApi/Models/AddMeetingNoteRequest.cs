namespace WebApi.Dtos
{
    public class AddMeetingNoteRequest
    {
        public int BookingId { get; set; }
        public string Content { get; set; }
    }
}
