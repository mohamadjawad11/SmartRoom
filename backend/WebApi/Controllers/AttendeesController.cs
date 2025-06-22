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
    public class AttendeesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AttendeesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendees()
        {
            var attendees = await _context.Attendees.ToListAsync();
            var attendeeDtos = _mapper.Map<List<AttendeeDto>>(attendees);
            return Ok(attendeeDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendee([FromBody] CreateAttendeeDto createAttendeeDto)
        {
            var attendee = _mapper.Map<Attendee>(createAttendeeDto);
            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();
            var attendeeDto = _mapper.Map<AttendeeDto>(attendee);
            return CreatedAtAction(nameof(GetAttendees), new { id = attendee.Id }, attendeeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendee(int id, [FromBody] UpdateAttendeeDto updateAttendeeDto)
        {
            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee == null) return NotFound();

            _mapper.Map(updateAttendeeDto, attendee);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendee(int id)
        {
            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee == null) return NotFound();

            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 