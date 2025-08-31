using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShelfWise.Models.Models
{

    [Index(nameof(UserId), nameof(Name), IsUnique = true)]
    public class Tag
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional: genre/mood/vibe, etc.</summary>
        public string? Kind { get; set; }

        // Nav
        public ICollection<BookTag> BookTags { get; set; } = new List<BookTag>();
    }

}
