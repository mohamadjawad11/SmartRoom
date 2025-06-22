using System;

namespace WebApi.Models.DTOs;

public class UpdateFileDto
{
    public int? MomId { get; set; }
    public int? UploadedByUser { get; set; }
    public string? FilePath { get; set; }
    public DateTime? UploadDate { get; set; }
} 