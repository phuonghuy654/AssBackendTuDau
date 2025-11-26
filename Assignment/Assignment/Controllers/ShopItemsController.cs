using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment.Models;
using Assignment.DTO;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopItemsController : ControllerBase
    {
        private readonly MinecraftDbContext _context;

        public ShopItemsController(MinecraftDbContext context)
        {
            _context = context;
        }

      
        [HttpPost("UploadImage/{id}")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
           
            var item = await _context.ShopItems.FindAsync(id);
            if (item == null) return NotFound("Item không tồn tại.");

         
            if (file == null || file.Length == 0)
            {
                return BadRequest("Vui lòng chọn file ảnh.");
            }

            try
            {
             
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

               
                var fileName = $"{Guid.NewGuid()}{file.FileName}";
                var filePath = Path.Combine(folderPath, fileName);

               
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

             
                item.Image = fileName; 
                await _context.SaveChangesAsync();

                
                
                var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";

                return Ok(new { Message = "Upload thành công", ImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        //3
       
        [HttpGet("ExpensiveWeapons")]
        public async Task<IActionResult> GetExpensiveWeapons()
        {
            var weapons = await _context.ShopItems
                .Where(x => x.ItemType == "Vũ khí" && x.PriceXp > 200)
                .ToListAsync();
            return Ok(weapons);
        }

        //4
    
        [HttpGet("Affordable/{characterId}")]
        public async Task<IActionResult> GetAffordableItems(int characterId)
        {
            
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null) return NotFound("Không tìm thấy nhân vật");

          
            var affordableItems = await _context.ShopItems
                .Where(x => x.PriceXp <= character.LevelXp)
                .ToListAsync();

            return Ok(new
            {
                CharacterXP = character.LevelXp,
                CanBuy = affordableItems
            });
        }

        //5
       
        [HttpGet("CheapDiamond")]
        public async Task<IActionResult> GetCheapDiamondItems()
        {
            var items = await _context.ShopItems
                .Where(x => x.ItemName.Contains("kim cương") && x.PriceXp < 100)
                .ToListAsync();
            return Ok(items);
        }

        //7

        [HttpPost]
        public async Task<IActionResult> CreateItem(ShopItemsDTO itemDTO)
        {

            var newItem = new ShopItem
            {
                ItemName = itemDTO.Name,
                ItemType = itemDTO.Type,
                PriceXp = itemDTO.PriceXp
            };
            _context.ShopItems.Add(newItem);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Thêm item thành công", Item = newItem });
        }

 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.ShopItems.ToListAsync());
        }




    }
}
