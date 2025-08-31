using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL; // for HasPostgresEnum
using ShelfWise.Models.Enums;                // Shelf, CardState
using ShelfWise.Models.Models;

namespace ShelfWise.Data
{

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        
        public DbSet<User> Users => Set<User>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<BookMeta> BookMeta => Set<BookMeta>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<BookTag> BookTags => Set<BookTag>();
        public DbSet<ReadingSession> ReadingSessions => Set<ReadingSession>();
        public DbSet<Highlight> Highlights => Set<Highlight>();
        public DbSet<FavoriteQuote> FavoriteQuotes => Set<FavoriteQuote>();
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<BookRecallCard> BookRecallCards => Set<BookRecallCard>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // PostgreSQL Enums
            // ---------------------------
            // Registers PG enum types and makes migrations create/alter them.
            modelBuilder.HasPostgresEnum<Shelf>();
            modelBuilder.HasPostgresEnum<CardState>();

            //// Optionally force column types (nice if you want stable type names)
            //modelBuilder.Entity<Book>()
            //    .Property(b => b.Shelf)
            //    .HasColumnType("shelf");        // will use the "shelf" PG enum

            //modelBuilder.Entity<Card>()
            //    .Property(c => c.State)
            //    .HasColumnType("cardstate");    // will use the "cardstate" PG enum

            // ---------------------------
            // Users
            // ---------------------------
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                // children: Books, Tags (configured on their side)
            });

            // ---------------------------
            // Books
            // ---------------------------
            modelBuilder.Entity<Book>(e =>
            {
                e.HasIndex(b => new { b.UserId, b.Title });
                e.HasIndex(b => new { b.UserId, b.Author });

                e.HasOne(b => b.User)
                    .WithMany(u => u.Books)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // 1:1 Book ↔ BookMeta (PK=FK on BookMeta.BookId)
                e.HasOne(b => b.Meta)
                    .WithOne(m => m.Book!)
                    .HasForeignKey<BookMeta>(m => m.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                // 1:0..1 Book ↔ BookRecallCard
                e.HasOne(b => b.BookRecallCard)
                    .WithOne(rc => rc.Book!)
                    .HasForeignKey<BookRecallCard>(rc => rc.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------------------------
            // BookMeta
            // ---------------------------
            modelBuilder.Entity<BookMeta>(e =>
            {
                e.HasKey(m => m.BookId);
                // Rating 0..5 check (DB-level guard)
                e.ToTable(t => t.HasCheckConstraint(
                    "CK_BookMeta_Rating", "\"Rating\" BETWEEN 0 AND 5"));
            });

            // ---------------------------
            // Tags
            // ---------------------------
            modelBuilder.Entity<Tag>(e =>
            {
                // Unique per-user tag name
                e.HasIndex(t => new { t.UserId, t.Name }).IsUnique();

                e.HasOne(t => t.User)
                    .WithMany(u => u.Tags)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------------------------
            // BookTags (join table for Books ↔ Tags)
            // ---------------------------
            modelBuilder.Entity<BookTag>(e =>
            {
                e.HasKey(bt => new { bt.BookId, bt.TagId });

                e.HasOne(bt => bt.Book)
                    .WithMany(b => b.BookTags)
                    .HasForeignKey(bt => bt.BookId)
                    .OnDelete(DeleteBehavior.Cascade); // delete join rows if book goes

                e.HasOne(bt => bt.Tag)
                    .WithMany(t => t.BookTags)
                    .HasForeignKey(bt => bt.TagId)
                    .OnDelete(DeleteBehavior.Cascade); // delete join rows if tag goes
            });

            // ---------------------------
            // ReadingSessions
            // ---------------------------
            modelBuilder.Entity<ReadingSession>(e =>
            {
                e.HasIndex(rs => new { rs.BookId, rs.StartAt });

                e.HasOne(rs => rs.User)
                    .WithMany()
                    .HasForeignKey(rs => rs.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(rs => rs.Book)
                    .WithMany(b => b.ReadingSessions)
                    .HasForeignKey(rs => rs.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------------------------
            // Highlights
            // ---------------------------
            modelBuilder.Entity<Highlight>(e =>
            {
                e.HasOne(h => h.User)
                    .WithMany()
                    .HasForeignKey(h => h.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(h => h.Book)
                    .WithMany(b => b.Highlights)
                    .HasForeignKey(h => h.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                // text[] mapping for Tags: Npgsql maps string[] <-> text[] automatically
                // (No extra config needed)
            });

            // ---------------------------
            // FavoriteQuotes
            // ---------------------------
            modelBuilder.Entity<FavoriteQuote>(e =>
            {
                e.HasOne(fq => fq.User)
                    .WithMany()
                    .HasForeignKey(fq => fq.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(fq => fq.Book)
                    .WithMany(b => b.FavoriteQuotes)
                    .HasForeignKey(fq => fq.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------------------------
            // Cards (SR for highlights)
            // ---------------------------
            modelBuilder.Entity<Card>(e =>
            {
                e.HasIndex(c => new { c.UserId, c.DueOn });
                e.HasIndex(c => c.HighlightId).IsUnique(); // one card per highlight (optional)

                e.HasOne(c => c.User)
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(c => c.Highlight)
                    .WithOne(h => h.Card)
                    .HasForeignKey<Card>(c => c.HighlightId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------------------------
            // BookRecallCards (title↔author SR)
            // ---------------------------
            modelBuilder.Entity<BookRecallCard>(e =>
            {
                e.HasIndex(rc => new { rc.UserId, rc.DueOn });
                e.HasIndex(rc => rc.BookId).IsUnique(); // at most one recall card per book

                e.HasOne(rc => rc.User)
                    .WithMany()
                    .HasForeignKey(rc => rc.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(rc => rc.Book)
                    .WithOne(b => b.BookRecallCard!)
                    .HasForeignKey<BookRecallCard>(rc => rc.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------------------------
            // AuditLog (immutable history)
            // ---------------------------
            modelBuilder.Entity<AuditLog>(e =>
            {
                e.HasIndex(a => new { a.EntityType, a.EntityId, a.At });
                e.HasIndex(a => new { a.UserId, a.At });

                e.HasOne(a => a.User)
                    .WithMany()
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // keep logs even if user is deleted
            });
        }
    }

}
