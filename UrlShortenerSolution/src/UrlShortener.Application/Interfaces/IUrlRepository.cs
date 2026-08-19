using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces
{
    public interface IUrlRepository
    {
        Task<ShortenedUrl> AddAsync(ShortenedUrl url);
        Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode);
        Task<bool> ShortCodeExistsAsync(string shortCode);
    }
}

