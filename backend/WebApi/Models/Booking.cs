using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string? RoomId { get; set; }

    public int? BookedByUser { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Status { get; set; }

    public virtual User? BookedByUserNavigation { get; set; }

    public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();

    public virtual Room? Room { get; set; }
}
