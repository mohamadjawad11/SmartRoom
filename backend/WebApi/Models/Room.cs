using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Room
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public string? Features { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
