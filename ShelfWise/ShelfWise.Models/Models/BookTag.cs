using Microsoft.EntityFrameworkCore;

namespace ShelfWise.Models.Models
{

    // Composite PK will be configured in DbContext, or use [PrimaryKey] in EF Core 8:
    // [PrimaryKey(nameof(BookId), nameof(TagId))]
    public class BookTag
    {
        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        public Guid TagId { get; set; }
        public Tag? Tag { get; set; }
    }

}
