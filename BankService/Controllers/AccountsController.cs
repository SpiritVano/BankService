using Microsoft.AspNetCore.Mvc;
using BankService.Models;
using BankService.Dtos;
using Microsoft.EntityFrameworkCore;
using BankService.Data;

namespace BankService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly BankDbContext _context;

        public AccountsController(BankDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            var accounts = await _context.Accounts
                .Select(a => new AccountDto(a.Id, a.UserId, a.AccountNumber, a.Balance))
                .ToListAsync();

            return Ok(accounts);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountDto>> GetById(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null) return NotFound();

            return Ok(new AccountDto(account.Id, account.UserId, account.AccountNumber, account.Balance));
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create(CreateAccountDto dto)
        {
            var account = new Account
            {
                UserId = dto.UserId,
                AccountNumber = $"ACC{Guid.NewGuid().ToString("N")[..6]}",
                Balance = dto.InitialBalance
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var result = new AccountDto(account.Id, account.UserId, account.AccountNumber, account.Balance);
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, result);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto dto)
        {
            var from = await _context.Accounts.FindAsync(dto.FromAccountId);
            var to = await _context.Accounts.FindAsync(dto.ToAccountId);

            if (from is null || to is null)
                return NotFound(new { error = "One or both accounts not found" });

            if (from.Balance < dto.Amount)
                return BadRequest(new { error = "Insufficient funds" });

            from.Balance -= dto.Amount;
            to.Balance += dto.Amount;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Transfer successful",
                fromBalance = from.Balance,
                toBalance = to.Balance
            });
        }
    }
}
