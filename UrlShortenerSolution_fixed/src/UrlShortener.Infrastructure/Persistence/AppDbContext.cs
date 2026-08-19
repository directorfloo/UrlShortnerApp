using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ShortenedUrl> ShortenedUrls => Set<ShortenedUrl>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>(entity =>
            {
                entity.HasIndex(s => s.ShortCode).IsUnique();
                entity.Property(s => s.ShortCode).HasMaxLength(20).IsRequired();
                entity.Property(s => s.OriginalUrl).HasMaxLength(2048).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

