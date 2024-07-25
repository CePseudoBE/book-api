using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,1)")]
        [Range(0, 10, ErrorMessage = "Value must be between 0 and 10.")]
        [RegularExpression(@"^(\d+(\.5)?)$", ErrorMessage = "Value must be a whole number or end with .5.")]
        public required decimal Note { get; set; }

        [Required]
        [StringLength(1024)]
        public required string Thought { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public required int UserId  { get; set; }

        public required User User { get; set; }

        public required int BookId  { get; set; }

        public required Book Book { get; set; }

    }
}
