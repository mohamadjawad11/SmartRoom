using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

using File = WebApi.Models.File;

namespace WebApi.Data
{
    public partial class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        // Constructor that accepts DbContextOptions and IConfiguration
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // Define DbSet properties for each table
        public virtual DbSet<ActionItem> ActionItems { get; set; }
        public virtual DbSet<Attendee> Attendees { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
        public virtual DbSet<MinutesOfMeeting> MinutesOfMeetings { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        // Configure the connection string from appsettings.json
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use the connection string from configuration
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        // Configure the model using Fluent API (if needed)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionItem>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ActionIt__3214EC075939225A");
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.HasOne(d => d.AssignedToUserNavigation).WithMany(p => p.ActionItems)
                    .HasForeignKey(d => d.AssignedToUser)
                    .HasConstraintName("FK__ActionIte__Assig__73BA3083");

                entity.HasOne(d => d.Mom).WithMany(p => p.ActionItems)
                    .HasForeignKey(d => d.MomId)
                    .HasConstraintName("FK__ActionIte__MomId__72C60C4A");
            });

            modelBuilder.Entity<Attendee>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Attendee__3214EC0742BA997C");
                entity.HasOne(d => d.Meeting).WithMany(p => p.Attendees)
                    .HasForeignKey(d => d.MeetingId)
                    .HasConstraintName("FK__Attendees__Meeti__68487DD7");

                entity.HasOne(d => d.User).WithMany(p => p.Attendees)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Attendees__UserI__6754599E");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Bookings__3214EC0779CC90C6");
                entity.Property(e => e.EndTime).HasColumnType("datetime");
                entity.Property(e => e.RoomId)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.BookedByUserNavigation).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookedByUser)
                    .HasConstraintName("FK__Bookings__Booked__60A75C0F");

                entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__Bookings__RoomId__5FB337D6");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Files__3214EC07FF1D0383");
                entity.Property(e => e.FilePath)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.Mom).WithMany(p => p.Files)
                    .HasForeignKey(d => d.MomId)
                    .HasConstraintName("FK__Files__MomId__6EF57B66");

                entity.HasOne(d => d.UploadedByUserNavigation).WithMany(p => p.Files)
                    .HasForeignKey(d => d.UploadedByUser)
                    .HasConstraintName("FK__Files__UploadedB__6FE99F9F");
            });

            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Meetings__3214EC07F9BFBF19");
                entity.Property(e => e.Agenda).HasColumnType("text");
                entity.Property(e => e.Title)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Booking).WithMany(p => p.Meetings)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Meetings__Bookin__6383C8BA");

                entity.HasOne(d => d.ScheduledByUserNavigation).WithMany(p => p.Meetings)
                    .HasForeignKey(d => d.ScheduledByUser)
                    .HasConstraintName("FK__Meetings__Schedu__6477ECF3");
            });

            modelBuilder.Entity<MinutesOfMeeting>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MinutesO__3214EC0747022633");
                entity.ToTable("MinutesOfMeeting");
                entity.Property(e => e.Decisions).HasColumnType("text");
                entity.Property(e => e.DiscussionPoints).HasColumnType("text");
                entity.Property(e => e.TimeCreation).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByUserNavigation).WithMany(p => p.MinutesOfMeetings)
                    .HasForeignKey(d => d.CreatedByUser)
                    .HasConstraintName("FK__MinutesOf__Creat__6C190EBB");

                entity.HasOne(d => d.Meeting).WithMany(p => p.MinutesOfMeetings)
                    .HasForeignKey(d => d.MeetingId)
                    .HasConstraintName("FK__MinutesOf__Meeti__6B24EA82");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC079935D12E");
                entity.Property(e => e.Body).HasColumnType("text");
                entity.Property(e => e.SentTime).HasColumnType("datetime");
                entity.Property(e => e.Subject).HasColumnType("text");
                entity.Property(e => e.Type)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ActionItem).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.ActionItemId)
                    .HasConstraintName("FK__Notificat__Actio__778AC167");

                entity.HasOne(d => d.Meeting).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.MeetingId)
                    .HasConstraintName("FK__Notificat__Meeti__76969D2E");

                entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__UserI__787EE5A0");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Rooms__3214EC076CEDDA64");
                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Features).HasColumnType("text");
                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Role).HasDefaultValue("");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
