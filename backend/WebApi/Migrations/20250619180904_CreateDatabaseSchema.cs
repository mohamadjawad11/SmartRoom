using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Features = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rooms__3214EC076CEDDA64", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    BookedByUser = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookings__3214EC0779CC90C6", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Bookings__Booked__60A75C0F",
                        column: x => x.BookedByUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Bookings__RoomId__5FB337D6",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    ScheduledByUser = table.Column<int>(type: "int", nullable: true),
                    Agenda = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Meetings__3214EC07F9BFBF19", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Meetings__Bookin__6383C8BA",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Meetings__Schedu__6477ECF3",
                        column: x => x.ScheduledByUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attendees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    MeetingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Attendee__3214EC0742BA997C", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Attendees__Meeti__68487DD7",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Attendees__UserI__6754599E",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MinutesOfMeeting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetingId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUser = table.Column<int>(type: "int", nullable: true),
                    TimeCreation = table.Column<DateTime>(type: "datetime", nullable: true),
                    Decisions = table.Column<string>(type: "text", nullable: true),
                    DiscussionPoints = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MinutesO__3214EC0747022633", x => x.Id);
                    table.ForeignKey(
                        name: "FK__MinutesOf__Creat__6C190EBB",
                        column: x => x.CreatedByUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__MinutesOf__Meeti__6B24EA82",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MomId = table.Column<int>(type: "int", nullable: true),
                    AssignedToUser = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ActionIt__3214EC075939225A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ActionIte__Assig__73BA3083",
                        column: x => x.AssignedToUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ActionIte__MomId__72C60C4A",
                        column: x => x.MomId,
                        principalTable: "MinutesOfMeeting",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MomId = table.Column<int>(type: "int", nullable: true),
                    UploadedByUser = table.Column<int>(type: "int", nullable: true),
                    FilePath = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Files__3214EC07FF1D0383", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Files__MomId__6EF57B66",
                        column: x => x.MomId,
                        principalTable: "MinutesOfMeeting",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Files__UploadedB__6FE99F9F",
                        column: x => x.UploadedByUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetingId = table.Column<int>(type: "int", nullable: true),
                    ActionItemId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SentTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Type = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__3214EC079935D12E", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Notificat__Actio__778AC167",
                        column: x => x.ActionItemId,
                        principalTable: "ActionItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Notificat__Meeti__76969D2E",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Notificat__UserI__787EE5A0",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_AssignedToUser",
                table: "ActionItems",
                column: "AssignedToUser");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_MomId",
                table: "ActionItems",
                column: "MomId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_MeetingId",
                table: "Attendees",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_UserId",
                table: "Attendees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookedByUser",
                table: "Bookings",
                column: "BookedByUser");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_MomId",
                table: "Files",
                column: "MomId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UploadedByUser",
                table: "Files",
                column: "UploadedByUser");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_BookingId",
                table: "Meetings",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ScheduledByUser",
                table: "Meetings",
                column: "ScheduledByUser");

            migrationBuilder.CreateIndex(
                name: "IX_MinutesOfMeeting_CreatedByUser",
                table: "MinutesOfMeeting",
                column: "CreatedByUser");

            migrationBuilder.CreateIndex(
                name: "IX_MinutesOfMeeting_MeetingId",
                table: "MinutesOfMeeting",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ActionItemId",
                table: "Notifications",
                column: "ActionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_MeetingId",
                table: "Notifications",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendees");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ActionItems");

            migrationBuilder.DropTable(
                name: "MinutesOfMeeting");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "");
        }
    }
}
