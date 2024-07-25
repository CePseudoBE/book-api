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
        private readonly EmailHelper _emailHelper = new();

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

        [HttpPost("ask-reset")]//todo email
        public async Task<IActionResult> AskForToken([FromBody] AskTokenDto request){
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var passwordReset = await _context.PasswordResets
                .Where(pr => pr.UserId == user.Id)
                .OrderByDescending(pr => pr.CreatedAt)
                .FirstOrDefaultAsync();
            
            if (passwordReset != null && passwordReset.Active && passwordReset.ExpiresAt > DateTime.UtcNow)
            {
                FormattableString toSend = $"Hello {user.FullName}, click here to reset your password : {request.RedirectUrl}/?token={passwordReset.Token}";

                _emailHelper.SendEmail("Book Application", "infofin@ulb.be", user.FullName, user.Email, "Reset password", toSend.ToString());
                
                return Ok("Email envoyé");
            }

            var newToken = new PasswordReset
            {
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                User = user,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            
            _context.PasswordResets.Add(newToken);
            await _context.SaveChangesAsync();

            FormattableString stringToSend = $"Hello {user.FullName}, click here to reset your password : {request.RedirectUrl}/?token={newToken.Token}";

            _emailHelper.SendEmail("Book Application", "infofin@ulb.be", user.FullName, user.Email, "Reset password", stringToSend.ToString());

            return Ok("Email envoyé");
        }

        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
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