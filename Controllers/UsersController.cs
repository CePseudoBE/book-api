using Microsoft.AspNetCore.Mvc;
using BookApi.Models;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> users = [];

        [HttpPost("register", Name = "RegisterUser")]        
        public IActionResult Register([FromBody] UserDto userDto)
        {
            if (users.Any(u => u.Username == userDto.Username))
            {
                return BadRequest("User already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                Id = users.Count + 1,
                FullName = userDto.FullName,
                Email = userDto.Email,
                Username = userDto.Username,
                Password = passwordHash
            };

            users.Add(user);

            return Ok("User registered successfully");
        }

        [HttpPost("login", Name = "LoginUser")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = users.SingleOrDefault(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return BadRequest("Invalid username");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest("Invalid password");
            }

            return Ok("Login successful");
        }
    }

    public class UserDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
