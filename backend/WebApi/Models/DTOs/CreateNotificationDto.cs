using System;

namespace WebApi.Models.DTOs;

public class CreateNotificationDto
{
    public int? MeetingId { get; set; }
    public int? ActionItemId { get; set; }
    public int? UserId { get; set; }
    public DateTime? SentTime { get; set; }
    public string? Type { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
} 