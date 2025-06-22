using System;

namespace WebApi.Models.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public string? RoomId { get; set; }
    public int? BookedByUser { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Status { get; set; }
} 