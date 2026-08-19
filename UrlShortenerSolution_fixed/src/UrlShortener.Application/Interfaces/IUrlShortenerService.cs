using UrlShortener.Application.Common;
using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<ServiceResult<ShortUrlResponseDto>> CreateShortUrlAsync(CreateShortUrlRequestDto request, string baseUrl);
        Task<ServiceResult<string>> ResolveAsync(string shortCode);
    }
}

