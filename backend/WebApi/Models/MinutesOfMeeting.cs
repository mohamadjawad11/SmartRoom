using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class MinutesOfMeeting
{
    public int Id { get; set; }

    public int? MeetingId { get; set; }

    public int? CreatedByUser { get; set; }

    public DateTime? TimeCreation { get; set; }

    public string? Decisions { get; set; }

    public string? DiscussionPoints { get; set; }

    public virtual ICollection<ActionItem> ActionItems { get; set; } = new List<ActionItem>();

    public virtual User? CreatedByUserNavigation { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual Meeting? Meeting { get; set; }
}
