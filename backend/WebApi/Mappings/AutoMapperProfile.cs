using AutoMapper;
using WebApi.Models;
using WebApi.Models.DTOs;

using FileEntity = WebApi.Models.File;

namespace WebApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<ActionItem, ActionItemDto>();
            CreateMap<CreateActionItemDto, ActionItem>();
            CreateMap<UpdateActionItemDto, ActionItem>();

            CreateMap<Attendee, AttendeeDto>();
            CreateMap<CreateAttendeeDto, Attendee>();
            CreateMap<UpdateAttendeeDto, Attendee>();

            CreateMap<Booking, BookingDto>();
            CreateMap<CreateBookingDto, Booking>();
            CreateMap<UpdateBookingDto, Booking>();

            CreateMap<FileEntity, FileDto>();
            CreateMap<CreateFileDto, FileEntity>();
            CreateMap<UpdateFileDto, FileEntity>();

            CreateMap<Meeting, MeetingDto>();
            CreateMap<CreateMeetingDto, Meeting>();
            CreateMap<UpdateMeetingDto, Meeting>();

            CreateMap<MinutesOfMeeting, MinutesOfMeetingDto>();
            CreateMap<CreateMinutesOfMeetingDto, MinutesOfMeeting>();
            CreateMap<UpdateMinutesOfMeetingDto, MinutesOfMeeting>();

            CreateMap<Notification, NotificationDto>();
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<UpdateNotificationDto, Notification>();

            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();
        }
    }
}
