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
    public class PurchasesController : ControllerBase
    {
        private readonly MinecraftDbContext _context;

        public PurchasesController(MinecraftDbContext context)
        {
            _context = context;
        }

        // YÊU CẦU 6: Lịch sử mua hàng của 1 người chơi, sắp xếp theo thời gian
        // GET: api/Purchases/History/1
        [HttpGet("History/{characterId}")]
        public async Task<IActionResult> GetHistory(int characterId)
        {
            var history = await _context.Purchases
                .Include(p => p.Item) // Kèm theo thông tin tên Item
                .Where(p => p.CharacterId == characterId)
                .OrderByDescending(p => p.DatePurchased) // Sắp xếp mới nhất lên đầu (hoặc OrderBy để cũ nhất lên đầu)
                .Select(p => new
                {
                    ItemName = p.Item.ItemName,
                    PriceAtPurchase = p.PriceAtPurchase,
                    Date = p.DatePurchased
                })
                .ToListAsync();

            if (!history.Any()) return NotFound("Nhân vật này chưa mua gì cả.");

            return Ok(history);
        }

        // 9. Lấy danh sách các item được mua nhiều nhất
        // GET: api/Purchases/MostPurchasedItems
        [HttpGet("MostPurchasedItems")]
        public async Task<ActionResult<IEnumerable<object>>> GetMostPurchasedItems()
        {
            var mostPurchased = await _context.Purchases
                .Include(p => p.Item) // Đảm bảo Item được tải để lấy ItemName
                .GroupBy(p => p.ItemId)
                .Select(g => new
                {
                    ItemId = g.Key,
                    ItemName = g.First().Item.ItemName,
                    PurchaseCount = g.Count()
                })
                .OrderByDescending(x => x.PurchaseCount)
                .ToListAsync();

            if (mostPurchased == null || !mostPurchased.Any())
            {
                return NotFound("Không tìm thấy dữ liệu mua hàng nào.");
            }

            return mostPurchased;
        }

        // 10. Lấy danh sách tất cả người chơi và số lần họ đã mua hàng
        // GET: api/Purchases/PlayerPurchaseCounts
        [HttpGet("PlayerPurchaseCounts")]
        public async Task<ActionResult<IEnumerable<object>>> GetPlayersPurchaseCounts()
        {
            // Cần Include Character và Account để truy cập Username
            var playerPurchaseCounts = await _context.Purchases
                .Include(p => p.Character)
                .ThenInclude(c => c.Account)
                .GroupBy(p => p.CharacterId)
                .Select(g => new
                {
                    CharacterId = g.Key,
                    Username = g.First().Character.Account.Username,
                    TotalPurchases = g.Count()
                })
                .OrderByDescending(x => x.TotalPurchases)
                .ToListAsync();

            if (playerPurchaseCounts == null || !playerPurchaseCounts.Any())
            {
                return NotFound("Không tìm thấy dữ liệu mua hàng nào.");
            }

            return playerPurchaseCounts;
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.PurchaseId == id);
        }




    }
}
