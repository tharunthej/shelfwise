using System.ComponentModel.DataAnnotations;

namespace ShelfWise.Models.Models
{

    public class Highlight
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public string? Location { get; set; }

        // PostgreSQL text[] — Npgsql maps to string[]
        public string[]? Tags { get; set; } = Array.Empty<string>();

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Nav
        public Card? Card { get; set; } // 0..1
    }

}
