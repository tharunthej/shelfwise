using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ShelfWise.Models.Enums;

namespace ShelfWise.Models.Models
{

    [Index(nameof(UserId), nameof(Title))]
    [Index(nameof(UserId), nameof(Author))]
    public class Book
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public string? Isbn { get; set; }
        public int? PageCount { get; set; }
        public int? PublishedYear { get; set; }

        public Shelf Shelf { get; set; } = Shelf.ToRead;

        public DateOnly? StartedOn { get; set; }
        public DateOnly? FinishedOn { get; set; }

        public string? CoverUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Nav
        public BookMeta? Meta { get; set; }
        public ICollection<BookTag> BookTags { get; set; } = new List<BookTag>();
        public ICollection<ReadingSession> ReadingSessions { get; set; } = new List<ReadingSession>();
        public ICollection<Highlight> Highlights { get; set; } = new List<Highlight>();
        public ICollection<FavoriteQuote> FavoriteQuotes { get; set; } = new List<FavoriteQuote>();
        public BookRecallCard? BookRecallCard { get; set; }
    }

}
