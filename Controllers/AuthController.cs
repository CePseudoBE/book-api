using Microsoft.AspNetCore.Mvc;
using BookApi.Data;
using BookApi.Helpers;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;
using Microsoft.AspNetCore.Authorization;
using BookApi.Validator;

namespace BookApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly PasswordHasher _passwordHasher = new();
        private readonly JWTHelper _jwtHelper = new();

        [HttpPost("register", Name = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterDto userDto)
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
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Token and new password must be provided.");
            }

            var passwordReset = await _context.PasswordResets
                .Include(pr => pr.User)
                .FirstOrDefaultAsync(pr => pr.Token == request.Token);

            if (passwordReset == null)
            {
                return BadRequest("Invalid token.");
            }

            if (!passwordReset.Active)
            {
                return BadRequest("Token has already been used.");
            }

            if (passwordReset.ExpiresAt <= DateTime.UtcNow)
            {
                passwordReset.Active = false;
                await _context.SaveChangesAsync();
                return BadRequest("Token has expired.");
            }

            passwordReset.User.Password = _passwordHasher.HashPassword(request.NewPassword);
            passwordReset.Active = false;
            passwordReset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok("Password has been reset successfully.");
        }
    }

}