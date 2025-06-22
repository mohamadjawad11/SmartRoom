using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class ActionItem
{
    public int Id { get; set; }

    public int? MomId { get; set; }

    public int? AssignedToUser { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int? Status { get; set; }

    public virtual User? AssignedToUserNavigation { get; set; }

    public virtual MinutesOfMeeting? Mom { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
