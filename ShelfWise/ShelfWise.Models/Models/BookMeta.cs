using System.ComponentModel.DataAnnotations;

namespace ShelfWise.Models.Models
{

    public class BookMeta
    {
        // PK == FK (true 1:1 with Book)
        [Key]
        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        [MaxLength(140)]
        public string? OneLinePitch { get; set; }

        public string? PlotSummary { get; set; }
        public string? PersonalReview { get; set; }

        /// <summary>0..5</summary>
        public short? Rating { get; set; }

        public string? MemoryHook { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
