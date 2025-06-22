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
    public class ActionItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActionItemsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetActionItems()
        {
            var actionItems = await _context.ActionItems.ToListAsync();
            var actionItemDtos = _mapper.Map<List<ActionItemDto>>(actionItems);
            return Ok(actionItemDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActionItem([FromBody] CreateActionItemDto createActionItemDto)
        {
            var actionItem = _mapper.Map<ActionItem>(createActionItemDto);
            _context.ActionItems.Add(actionItem);
            await _context.SaveChangesAsync();
            var actionItemDto = _mapper.Map<ActionItemDto>(actionItem);
            return CreatedAtAction(nameof(GetActionItems), new { id = actionItem.Id }, actionItemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActionItem(int id, [FromBody] UpdateActionItemDto updateActionItemDto)
        {
            var actionItem = await _context.ActionItems.FindAsync(id);
            if (actionItem == null) return NotFound();

            _mapper.Map(updateActionItemDto, actionItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionItem(int id)
        {
            var actionItem = await _context.ActionItems.FindAsync(id);
            if (actionItem == null) return NotFound();

            _context.ActionItems.Remove(actionItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 