using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string FullName { get; set; }

        public required int Born { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Book>? Books { get; set; }
    }
}
