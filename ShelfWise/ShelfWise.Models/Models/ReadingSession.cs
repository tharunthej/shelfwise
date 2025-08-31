using Microsoft.EntityFrameworkCore;

namespace ShelfWise.Models.Models
{

    [Index(nameof(BookId), nameof(StartAt))]
    public class ReadingSession
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset? EndAt { get; set; }

        public int? PagesRead { get; set; }
        public string? Notes { get; set; }
    }

}
