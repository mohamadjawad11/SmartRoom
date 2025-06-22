using System;

namespace WebApi.Models.DTOs;

public class FileDto
{
    public int Id { get; set; }
    public int? MomId { get; set; }
    public int? UploadedByUser { get; set; }
    public string? FilePath { get; set; }
    public DateTime? UploadDate { get; set; }
} 