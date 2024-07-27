using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserBook>? UserBooks { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        
        public ICollection<PasswordReset>? PasswordResets { get; set; }
    }
}
