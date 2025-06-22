namespace WebApi.Models.DTOs;

public class CreateRoomDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public string? Features { get; set; }
} 