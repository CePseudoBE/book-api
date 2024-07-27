using System.ComponentModel.DataAnnotations;

namespace BookApi.Models
{
    public class UserBook
    {
        public int UserId { get; set; }
        public required User User { get; set; }

        public int BookId { get; set; }
        public required Book Book { get; set; }

        [Required]
        public bool IsRead { get; set; }
    }
}
