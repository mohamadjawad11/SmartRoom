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
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RoomsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            var roomDtos = _mapper.Map<List<RoomDto>>(rooms);
            return Ok(roomDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto createRoomDto)
        {
            var room = _mapper.Map<Room>(createRoomDto);
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            var roomDto = _mapper.Map<RoomDto>(room);
            return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, roomDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(string id, [FromBody] UpdateRoomDto updateRoomDto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _mapper.Map(updateRoomDto, room);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 