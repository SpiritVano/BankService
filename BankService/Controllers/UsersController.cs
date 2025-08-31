using Microsoft.AspNetCore.Mvc;
using BankService.Models;
using BankService.Dtos;
using Microsoft.EntityFrameworkCore;
using BankService.Data;

namespace BankService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BankDbContext _context;

        public UsersController(BankDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _context.Users
                .Select(u => new UserDto(u.Id, u.Name, u.Email))
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return NotFound();

            return Ok(new UserDto(user.Id, user.Name, user.Email));
        }

        //public record CreateUserDto(string Name, string Email);

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict(new { error = "Email already exists" });

            var user = new User { Name = dto.Name, Email = dto.Email };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new UserDto(user.Id, user.Name, user.Email);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, result);
        }

        //public record UpdateUserDto(string Name);

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return NotFound();

            user.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
