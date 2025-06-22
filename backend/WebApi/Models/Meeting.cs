using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Meeting
{
    public int Id { get; set; }

    public int? BookingId { get; set; }

    public int? ScheduledByUser { get; set; }

    public string? Agenda { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

    public virtual Booking? Booking { get; set; }

    public virtual ICollection<MinutesOfMeeting> MinutesOfMeetings { get; set; } = new List<MinutesOfMeeting>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual User? ScheduledByUserNavigation { get; set; }
}
