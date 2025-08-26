using Microsoft.AspNetCore.Mvc;
using BankService.Models;
using BankService.Dtos;

namespace BankService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private static readonly List<Account> _accounts = new()
        {
            new Account { Id = 1, UserId = 1, AccountNumber = "ACC1001", Balance = 5000m },
            new Account { Id = 2, UserId = 1, AccountNumber = "ACC1002", Balance = 2000m }
        };

        private static int _nextId = 3;

        [HttpGet]
        public ActionResult<IEnumerable<AccountDto>> GetAll()
        {
            var accounts = _accounts.Select(a =>
                new AccountDto(a.Id, a.UserId, a.AccountNumber, a.Balance));
            return Ok(accounts);
        }

        [HttpGet("{id:int}")]
        public ActionResult<AccountDto> GetById(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            if (account is null) return NotFound();

            return Ok(new AccountDto(account.Id, account.UserId, account.AccountNumber, account.Balance));
        }

        [HttpPost]
        public ActionResult<AccountDto> Create(CreateAccountDto dto)
        {
            var account = new Account
            {
                Id = _nextId++,
                UserId = dto.UserId,
                AccountNumber = $"ACC{1000 + _nextId}",
                Balance = dto.InitialBalance
            };

            _accounts.Add(account);

            var result = new AccountDto(account.Id, account.UserId, account.AccountNumber, account.Balance);
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, result);
        }

        [HttpPost("transfer")]
        public IActionResult Transfer(TransferDto dto)
        {
            var from = _accounts.FirstOrDefault(a => a.Id == dto.FromAccountId);
            var to = _accounts.FirstOrDefault(a => a.Id == dto.ToAccountId);

            if (from is null || to is null)
                return NotFound(new { error = "One or both accounts not found" });

            if (from.Balance < dto.Amount)
                return BadRequest(new { error = "Insufficient funds" });

            // Выполним перевод
            from.Balance -= dto.Amount;
            to.Balance += dto.Amount;

            return Ok(new
            {
                message = "Transfer successful",
                fromBalance = from.Balance,
                toBalance = to.Balance
            });
        }
    }
}
