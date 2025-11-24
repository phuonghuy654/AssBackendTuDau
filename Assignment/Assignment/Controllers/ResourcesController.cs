using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment.Models;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase 
    {
        private readonly MinecraftDbContext _context;

        public ResourcesController(MinecraftDbContext context)
        {
            _context = context;
        }

        //1      
        [HttpGet]
        public async Task<IActionResult> GetResources()
        {
            var resources = await _context.Resources.ToListAsync();
            return Ok(resources); 
        }

        //2
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound("Không tìm thấy tài nguyên này.");
            }

            return Ok(resource);
        }

        //3
       
        [HttpPost]
        public async Task<IActionResult> PostResource(Resource resource)
        {
           
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.ResourceId }, resource);
        }

        // 4
      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource(int id, Resource resource)
        {
            if (id != resource.ResourceId)
            {
                return BadRequest("ID không khớp.");
            }

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       
           
        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.ResourceId == id);
        }



    }
}