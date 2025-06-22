using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.DTOs;
using AutoMapper;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MeetingsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            var meetings = await _context.Meetings.ToListAsync();
            var meetingDtos = _mapper.Map<List<MeetingDto>>(meetings);
            return Ok(meetingDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingDto createMeetingDto)
        {
            var meeting = _mapper.Map<Meeting>(createMeetingDto);
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            var meetingDto = _mapper.Map<MeetingDto>(meeting);
            return CreatedAtAction(nameof(GetMeetings), new { id = meeting.Id }, meetingDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeeting(int id, [FromBody] UpdateMeetingDto updateMeetingDto)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null) return NotFound();

            _mapper.Map(updateMeetingDto, meeting);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null) return NotFound();

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 