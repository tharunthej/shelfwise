using System.ComponentModel.DataAnnotations;

namespace ShelfWise.Models.Models
{

    public class FavoriteQuote
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public string? PageOrLocation { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
