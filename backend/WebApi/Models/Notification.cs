using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int? MeetingId { get; set; }

    public int? ActionItemId { get; set; }

    public int? UserId { get; set; }

    public DateTime? SentTime { get; set; }

    public string? Type { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public virtual ActionItem? ActionItem { get; set; }

    public virtual Meeting? Meeting { get; set; }

    public virtual User? User { get; set; }
}
