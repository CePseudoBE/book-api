using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string Title { get; set; }

        [Required]
        [StringLength(13)]
        public required string ISBN { get; set; }

        public int VolumeNumber { get; set; }
        
        public int PageCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User>? Users { get; set; }

        public required int AuthorId {get; set;}

        public required Author Author {get; set;}
        
        public ICollection<Review>? Reviews { get; set; }
    }
}
