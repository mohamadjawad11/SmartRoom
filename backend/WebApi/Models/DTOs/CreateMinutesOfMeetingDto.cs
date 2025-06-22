using System;

namespace WebApi.Models.DTOs;

public class CreateMinutesOfMeetingDto
{
    public int? MeetingId { get; set; }
    public int? CreatedByUser { get; set; }
    public DateTime? TimeCreation { get; set; }
    public string? Decisions { get; set; }
    public string? DiscussionPoints { get; set; }
} 