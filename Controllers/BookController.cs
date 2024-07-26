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
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LoggingHelper _loggingHelper;

        public BookController(AppDbContext context, LoggingHelper loggingHelper)
        {
            _context = context;
            _loggingHelper = loggingHelper;
        }

        [HttpPost("", Name = "CreateBooks")]
        public IActionResult Create([FromBody] BookDto request)
        {
            try
            {
                _loggingHelper.LogInformation(request.ISBN);
                return Ok("test");
            }
            catch (Exception ex)
            {
                _loggingHelper.LogError(ex, "An error occurred while registering the user.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
