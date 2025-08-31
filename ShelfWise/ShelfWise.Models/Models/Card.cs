using Microsoft.EntityFrameworkCore;
using ShelfWise.Models.Enums;

namespace ShelfWise.Models.Models
{

    [Index(nameof(UserId), nameof(DueOn))]
    [Index(nameof(HighlightId), IsUnique = true)] // one card per highlight (optional)
    public class Card
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid HighlightId { get; set; }
        public Highlight? Highlight { get; set; }

        public CardState State { get; set; } = CardState.New;

        public int IntervalDays { get; set; } = 0;
        public double Ease { get; set; } = 2.5;

        // Store as DATE in PG; EF maps DateOnly via Npgsql provider
        public DateOnly? DueOn { get; set; }

        public int Reps { get; set; } = 0;
        public int Lapses { get; set; } = 0;
        public short? LastGrade { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
