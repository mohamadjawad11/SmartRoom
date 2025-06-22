using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class File
{
    public int Id { get; set; }

    public int? MomId { get; set; }

    public int? UploadedByUser { get; set; }

    public string? FilePath { get; set; }

    public DateTime? UploadDate { get; set; }

    public virtual MinutesOfMeeting? Mom { get; set; }

    public virtual User? UploadedByUserNavigation { get; set; }
}
