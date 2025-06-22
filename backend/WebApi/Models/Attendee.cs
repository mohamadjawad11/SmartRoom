using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Attendee
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? MeetingId { get; set; }

    public virtual Meeting? Meeting { get; set; }

    public virtual User? User { get; set; }
}
