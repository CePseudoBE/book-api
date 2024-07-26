using Microsoft.AspNetCore.Mvc;
using BookApi.Data;
using BookApi.Helpers;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;
using BookApi.Validator;
using System;
using System.Threading.Tasks;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly JWTHelper _jwtHelper;
        private readonly LoggingHelper _loggingHelper;
        private readonly EmailHelper _emailHelper;

        public AuthController(AppDbContext context, LoggingHelper loggingHelper, EmailHelper emailHelper)
        {
            _context = context;
            _passwordHasher = new PasswordHasher();
            _jwtHelper = new JWTHelper();
            _loggingHelper = loggingHelper;
            _emailHelper = emailHelper;
        }

        [HttpPost("register", Name = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterDto userDto)
        {
            try
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
            catch (Exception ex)
            {
                _loggingHelper.LogError(ex, "An error occurred while registering the user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login", Name = "LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
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
            catch (Exception ex)
            {
                _loggingHelper.LogError(ex, "An error occurred while logging in the user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("ask-reset")]
        public async Task<IActionResult> AskForToken([FromBody] AskTokenDto request)
        {
            try
            {
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
                    
                    return Ok("Email sent");
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

                return Ok("Email sent");
            }
            catch (Exception ex)
            {
                _loggingHelper.LogError(ex, "An error occurred while requesting a password reset token.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            try
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
            catch (Exception ex)
            {
                _loggingHelper.LogError(ex, "An error occurred while resetting the password.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
