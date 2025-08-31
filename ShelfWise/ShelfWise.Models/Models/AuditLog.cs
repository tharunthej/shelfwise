using Microsoft.EntityFrameworkCore;

namespace ShelfWise.Models.Models
{

    [Index(nameof(EntityType), nameof(EntityId), nameof(At))]
    [Index(nameof(UserId), nameof(At))]
    public class AuditLog
    {
        public long Id { get; set; } // bigserial

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public string EntityType { get; set; } = string.Empty; // "book", "highlight", "card", ...
        public Guid EntityId { get; set; }

        public string Action { get; set; } = string.Empty; // "create", "update", "delete", "review", ...

        public DateTimeOffset At { get; set; } = DateTimeOffset.UtcNow;

        // json/jsonb -> map as string or JsonDocument; keep string for simplicity
        public string? Payload { get; set; }
    }

}
