using System;

namespace WebApi.Models.DTOs;

public class CreateActionItemDto
{
    public int? MomId { get; set; }
    public int? AssignedToUser { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int? Status { get; set; }
} 