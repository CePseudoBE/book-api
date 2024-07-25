using System;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Token { get; set; }

        [Required]
        public required string UserId { get; set; }

        public required User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public bool Active { get; set; } = true;
    }
}
