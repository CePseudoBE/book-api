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
    public class UsersController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

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

        [Authorize]
        [HttpGet("", Name = "UsersList")]
        public async Task<IActionResult> GetUsersList()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Username = u.Username
                })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id){
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        Username = u.Username
                    })
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }

    

    
}
