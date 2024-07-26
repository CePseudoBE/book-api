using Microsoft.AspNetCore.Mvc;
using BookApi.Data;
using BookApi.Helpers;
using BookApi.Validator;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LoggingHelper _loggingHelper;
        private readonly IHttpClientFactory _httpClientFactory;


        public BookController(AppDbContext context, LoggingHelper loggingHelper, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _loggingHelper = loggingHelper;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("", Name = "CreateBooks")]
        public async Task<IActionResult> Create([FromBody] BookDto request)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"https://www.googleapis.com/books/v1/volumes?q=isbn:{request.ISBN}")
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/+json" },
                        { HeaderNames.UserAgent, "HttpRequestsSample" }
                    }
                };
                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();
                    
                    var googleBooksResponse = await JsonSerializer.DeserializeAsync<GoogleBooksResponse>(contentStream);
                    return Ok(googleBooksResponse);
                }
                return Ok("non");
            }
            catch (Exception ex)
            {
                _loggingHelper.LogError(ex, "An error occurred while creating a book.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
