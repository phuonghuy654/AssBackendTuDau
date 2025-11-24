using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment.Models;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly MinecraftDbContext _context;

        public CharactersController(MinecraftDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetByMode/{mode}")]
        public async Task<IActionResult> GetByMode(string mode)
        {
            
            var characters = await _context.Characters
                .Where(c => c.Mode == mode)
                .ToListAsync();

            if (characters.Count == 0)
            {
                return NotFound($"Không tìm thấy người chơi nào ở chế độ {mode}");
            }

            return Ok(characters);
        }
    }
}
