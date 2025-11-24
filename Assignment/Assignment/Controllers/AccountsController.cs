using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.DTO;
using Assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly MinecraftDbContext _context;

        public AccountsController(MinecraftDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        


      
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDto)
        {
            var account = await _context.Accounts.FindAsync(changePasswordDto.AccountId);

            if (account == null)
            {
               
                return NotFound(new { Message = "Account not found." });
            }

            if (account.Password != changePasswordDto.OldPassword)
            {
                
                return BadRequest(new { Message = "Invalid old password." });
            }

            account.Password = changePasswordDto.NewPassword;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.AccountId))
                {
                    return NotFound(new { Message = "Account was deleted by another operation." });
                }
                else
                {
                    throw;
                }
            }

    
            return Ok(new { Message = "Password updated successfully." });
        }



        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}