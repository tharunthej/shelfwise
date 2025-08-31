using Microsoft.EntityFrameworkCore;

namespace ShelfWise.Models.Models
{

    [Index(nameof(UserId), nameof(DueOn))]
    [Index(nameof(BookId), IsUnique = true)] // at most one recall card per book
    public class BookRecallCard
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        public int IntervalDays { get; set; } = 0;
        public double Ease { get; set; } = 2.5;

        public DateOnly? DueOn { get; set; }

        public int Reps { get; set; } = 0;
        public int Lapses { get; set; } = 0;
        public short? LastGrade { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}

