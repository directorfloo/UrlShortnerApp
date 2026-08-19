using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Persistence;

namespace UrlShortener.Infrastructure.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly AppDbContext _context;

        public UrlRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ShortenedUrl> AddAsync(ShortenedUrl url)
        {
            _context.ShortenedUrls.Add(url);
            await _context.SaveChangesAsync();
            return url;
        }

        public async Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode)
        {
            return await _context.ShortenedUrls
                .FirstOrDefaultAsync(s => s.ShortCode == shortCode);
        }

        public async Task<bool> ShortCodeExistsAsync(string shortCode)
        {
            return await _context.ShortenedUrls.AnyAsync(s => s.ShortCode == shortCode);
        }
    }
}

