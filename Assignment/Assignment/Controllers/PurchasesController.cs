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

        //6
       
        [HttpGet("History/{characterId}")]
        public async Task<IActionResult> GetHistory(int characterId)
        {
            var history = await _context.Purchases
                .Include(p => p.Item) 
                .Where(p => p.CharacterId == characterId)
                .OrderByDescending(p => p.DatePurchased) 
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

        // 9
      
        [HttpGet("MostPurchasedItems")]
        public async Task<ActionResult<IEnumerable<object>>> GetMostPurchasedItems()
        {
            var mostPurchased = await _context.Purchases
                .Include(p => p.Item) 
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

        // 10
      
        [HttpGet("PlayerPurchaseCounts")]
        public async Task<ActionResult<IEnumerable<object>>> GetPlayersPurchaseCounts()
        {
          
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
