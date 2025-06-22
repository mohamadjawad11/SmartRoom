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
    public class MinutesOfMeetingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MinutesOfMeetingController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMinutesOfMeetings()
        {
            var minutesOfMeetings = await _context.MinutesOfMeetings.ToListAsync();
            var minutesOfMeetingDtos = _mapper.Map<List<MinutesOfMeetingDto>>(minutesOfMeetings);
            return Ok(minutesOfMeetingDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMinutesOfMeeting([FromBody] CreateMinutesOfMeetingDto createMinutesOfMeetingDto)
        {
            var minutesOfMeeting = _mapper.Map<MinutesOfMeeting>(createMinutesOfMeetingDto);
            _context.MinutesOfMeetings.Add(minutesOfMeeting);
            await _context.SaveChangesAsync();
            var minutesOfMeetingDto = _mapper.Map<MinutesOfMeetingDto>(minutesOfMeeting);
            return CreatedAtAction(nameof(GetMinutesOfMeetings), new { id = minutesOfMeeting.Id }, minutesOfMeetingDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMinutesOfMeeting(int id, [FromBody] UpdateMinutesOfMeetingDto updateMinutesOfMeetingDto)
        {
            var minutesOfMeeting = await _context.MinutesOfMeetings.FindAsync(id);
            if (minutesOfMeeting == null) return NotFound();

            _mapper.Map(updateMinutesOfMeetingDto, minutesOfMeeting);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMinutesOfMeeting(int id)
        {
            var minutesOfMeeting = await _context.MinutesOfMeetings.FindAsync(id);
            if (minutesOfMeeting == null) return NotFound();

            _context.MinutesOfMeetings.Remove(minutesOfMeeting);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 