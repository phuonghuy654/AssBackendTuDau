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
    public class ResourcesController : ControllerBase // Dùng ControllerBase cho API sạch hơn
    {
        private readonly MinecraftDbContext _context;

        public ResourcesController(MinecraftDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách tất cả tài nguyên
        // GET: api/Resources
        [HttpGet]
        public async Task<IActionResult> GetResources()
        {
            var resources = await _context.Resources.ToListAsync();
            return Ok(resources); // Trả về JSON 200 OK
        }

        // 2. Lấy chi tiết 1 tài nguyên theo ID
        // GET: api/Resources/5
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

        // 3. Thêm mới tài nguyên (Ví dụ: Admin thêm loại khoáng sản mới)
        // POST: api/Resources
        [HttpPost]
        public async Task<IActionResult> PostResource(Resource resource)
        {
            // Entity Framework tự động bỏ qua ResourceId nếu nó là Identity (tự tăng)
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.ResourceId }, resource);
        }

        // 4. Sửa đổi tài nguyên
        // PUT: api/Resources/5
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

            return NoContent(); // 204 No Content (Sửa thành công nhưng không trả về dữ liệu gì)
        }

        // 5. Xóa tài nguyên
        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.ResourceId == id);
        }



    }
}