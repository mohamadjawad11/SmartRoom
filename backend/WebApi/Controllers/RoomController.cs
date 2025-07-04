using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/room
        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return Ok(rooms);
        }

        // GET: api/room/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound(new { message = "Room not found" });

            return Ok(room);
        }

        // POST: api/room
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        // PUT: api/room/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound(new { message = "Room not found" });

            room.Name = updatedRoom.Name;
            room.Capacity = updatedRoom.Capacity;
            room.Location = updatedRoom.Location;

            await _context.SaveChangesAsync();
            return Ok(room);
        }

        // DELETE: api/room/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound(new { message = "Room not found" });

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Room deleted successfully" });
        }
    }
}
