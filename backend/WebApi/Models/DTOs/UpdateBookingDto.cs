using System;

namespace WebApi.Models.DTOs;

public class UpdateBookingDto
{
    public string? RoomId { get; set; }
    public int? BookedByUser { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Status { get; set; }
} 