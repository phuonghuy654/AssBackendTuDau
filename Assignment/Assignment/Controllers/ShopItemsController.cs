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
    public class ShopItemsController : ControllerBase
    {
        private readonly MinecraftDbContext _context;

        public ShopItemsController(MinecraftDbContext context)
        {
            _context = context;
        }

        // YÊU CẦU 3: Lấy vũ khí có giá trị > 100 XP
        // GET: api/ShopItems/ExpensiveWeapons
        [HttpGet("ExpensiveWeapons")]
        public async Task<IActionResult> GetExpensiveWeapons()
        {
            var weapons = await _context.ShopItems
                .Where(x => x.ItemType == "Vũ khí" && x.PriceXp > 30)
                .ToListAsync();
            return Ok(weapons);
        }

        // YÊU CẦU 4: Lấy item người chơi CÓ THỂ MUA (Dựa trên XP hiện tại)
        // GET: api/ShopItems/Affordable/1
        [HttpGet("Affordable/{characterId}")]
        public async Task<IActionResult> GetAffordableItems(int characterId)
        {
            // B1: Lấy thông tin nhân vật để biết nó có bao nhiêu XP
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null) return NotFound("Không tìm thấy nhân vật");

            // B2: So sánh giá tiền item <= XP của nhân vật
            var affordableItems = await _context.ShopItems
                .Where(x => x.PriceXp <= character.LevelXp)
                .ToListAsync();

            return Ok(new
            {
                CharacterXP = character.LevelXp,
                CanBuy = affordableItems
            });
        }

        // YÊU CẦU 5: Item có tên chứa 'kim cương' và giá < 500
        // GET: api/ShopItems/CheapDiamond
        [HttpGet("CheapDiamond")]
        public async Task<IActionResult> GetCheapDiamondItems()
        {
            var items = await _context.ShopItems
                .Where(x => x.ItemName.Contains("kim cương") && x.PriceXp < 100)
                .ToListAsync();
            return Ok(items);
        }

        // YÊU CẦU 7: Thêm thông tin item mới
        // POST: api/ShopItems
        [HttpPost]
        public async Task<IActionResult> CreateItem(ShopItem item)
        {
            _context.ShopItems.Add(item);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Thêm item thành công", Item = item });
        }

        // Bonus: Lấy tất cả item (để test)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.ShopItems.ToListAsync());
        }


    }
}
