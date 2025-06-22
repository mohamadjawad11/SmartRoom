using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.DTOs;
using AutoMapper;
using FileEntity = WebApi.Models.File;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FilesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _context.Files.ToListAsync();
            var fileDtos = _mapper.Map<List<FileDto>>(files);
            return Ok(fileDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile([FromBody] CreateFileDto createFileDto)
        {
            var file = _mapper.Map<FileEntity>(createFileDto);
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            var fileDto = _mapper.Map<FileDto>(file);
            return CreatedAtAction(nameof(GetFiles), new { id = file.Id }, fileDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFile(int id, [FromBody] UpdateFileDto updateFileDto)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null) return NotFound();

            _mapper.Map(updateFileDto, file);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null) return NotFound();

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
