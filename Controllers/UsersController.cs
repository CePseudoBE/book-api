using Microsoft.AspNetCore.Mvc;
using BookApi.Data;
using BookApi.Helpers;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher _passwordHasher = new();
        private readonly JWTHelper _jwtHelper = new();

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register", Name = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (_context.Users.Any(u => u.Username == userDto.Username) || _context.Users.Any(u => u.Email == userDto.Email))
            {
                return BadRequest("User already exists");
            }

            var passwordHash = _passwordHasher.HashPassword(userDto.Password);

            var user = new User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                Username = userDto.Username,
                Password = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtHelper.GenerateJwtToken(user.Id.ToString());

            return Ok(new { Token = token });
        }

        [HttpPost("login", Name = "LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return BadRequest("Invalid username");
            }

            if (!_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
            {
                return BadRequest("Invalid password");
            }

            var token = _jwtHelper.GenerateJwtToken(user.Id.ToString());

            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("profile", Name = "UserProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { user.FullName, user.Email, user.Username });
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
