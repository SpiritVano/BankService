using Microsoft.AspNetCore.Mvc;
using BankService.Models;

namespace BankService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // Временное in-memory хранилище для обучения
        private static readonly List<User> _users = new()
    {
        new User { Id = 1, Name = "Ivan", Email = "ivan@example.com" }
    };

        private static int _nextId = 2;

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll() => Ok(_users);

        [HttpGet("{id:int}")]
        public ActionResult<User> GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        public record CreateUserDto(string Name, string Email);

        [HttpPost]
        public ActionResult<User> Create(CreateUserDto dto)
        {
            if (_users.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
                return Conflict(new { error = "Email already exists" });

            var user = new User { Id = _nextId++, Name = dto.Name, Email = dto.Email };
            _users.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        public record UpdateUserDto(string Name);

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateUserDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound();
            user.Name = dto.Name;
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) return NotFound();
            _users.Remove(user);
            return NoContent();
        }
    }

}
