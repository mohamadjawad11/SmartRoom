namespace WebApi.Models.DTOs;

public class CreateMeetingDto
{
    public int? BookingId { get; set; }
    public int? ScheduledByUser { get; set; }
    public string? Agenda { get; set; }
    public string? Title { get; set; }
} 