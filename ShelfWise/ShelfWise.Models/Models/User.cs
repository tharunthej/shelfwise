using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShelfWise.Models.Models
{

    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public Guid Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? DisplayName { get; set; }
        public string? Timezone { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Nav
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }

}

