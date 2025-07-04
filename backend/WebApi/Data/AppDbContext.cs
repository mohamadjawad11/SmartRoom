using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MeetingAttendee> MeetingAttendees { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<MeetingAttendee>()
        .HasOne(ma => ma.Booking)
        .WithMany(b => b.Attendees) // ✅ must match Booking.cs property name
        .HasForeignKey(ma => ma.BookingId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<MeetingAttendee>()
        .HasOne(ma => ma.User)
        .WithMany()
        .HasForeignKey(ma => ma.UserId)
        .OnDelete(DeleteBehavior.Restrict);
}

    }
}
