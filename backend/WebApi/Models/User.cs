using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<ActionItem> ActionItems { get; set; } = new List<ActionItem>();

    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();

    public virtual ICollection<MinutesOfMeeting> MinutesOfMeetings { get; set; } = new List<MinutesOfMeeting>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
