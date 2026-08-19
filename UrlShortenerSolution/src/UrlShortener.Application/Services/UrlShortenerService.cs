using System.Security.Cryptography;
using UrlShortener.Application.Common;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Services
{
    /// <summary>
    /// Business logic for creating and resolving shortened URLs. Open to
    /// anyone - there is no owner/auth concept here.
    /// </summary>
    public class UrlShortenerService : IUrlShortenerService
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int DefaultCodeLength = 7;
        private const int MaxGenerationAttempts = 10;

        private readonly IUrlRepository _urlRepository;

        public UrlShortenerService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<ServiceResult<ShortUrlResponseDto>> CreateShortUrlAsync(
            CreateShortUrlRequestDto request, string baseUrl)
        {
            string shortCode;

            if (!string.IsNullOrWhiteSpace(request.CustomCode))
            {
                shortCode = request.CustomCode.Trim();

                if (!IsValidCustomCode(shortCode))
                {
                    return ServiceResult<ShortUrlResponseDto>.Fail(
                        "Custom code may only contain letters, digits, hyphens, or underscores (3-20 chars).");
                }

                if (await _urlRepository.ShortCodeExistsAsync(shortCode))
                {
                    return ServiceResult<ShortUrlResponseDto>.Fail("That custom code is already in use.");
                }
            }
            else
            {
                shortCode = await GenerateUniqueCodeAsync();
            }

            var entity = new ShortenedUrl
            {
                OriginalUrl = request.OriginalUrl.Trim(),
                ShortCode = shortCode,
                CreatedAtUtc = DateTime.UtcNow,
            };

            var created = await _urlRepository.AddAsync(entity);

            return ServiceResult<ShortUrlResponseDto>.Ok(MapToDto(created, baseUrl));
        }

        public async Task<ServiceResult<string>> ResolveAsync(string shortCode)
        {
            var url = await _urlRepository.GetByShortCodeAsync(shortCode);

            if (url is null)
            {
                return ServiceResult<string>.Fail("Short URL not found.");
            }

            return ServiceResult<string>.Ok(url.OriginalUrl);
        }

        private async Task<string> GenerateUniqueCodeAsync()
        {
            for (var attempt = 0; attempt < MaxGenerationAttempts; attempt++)
            {
                var code = GenerateRandomCode(DefaultCodeLength);
                if (!await _urlRepository.ShortCodeExistsAsync(code))
                {
                    return code;
                }
            }

            throw new InvalidOperationException("Could not generate a unique short code, please try again.");
        }

        private static string GenerateRandomCode(int length)
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                chars[i] = AllowedChars[bytes[i] % AllowedChars.Length];
            }
            return new string(chars);
        }

        private static bool IsValidCustomCode(string code)
        {
            if (code.Length is < 3 or > 20)
            {
                return false;
            }
            return code.All(c => char.IsLetterOrDigit(c) || c is '-' or '_');
        }

        private static ShortUrlResponseDto MapToDto(ShortenedUrl entity, string baseUrl)
        {
            return new ShortUrlResponseDto
            {
                ShortCode = entity.ShortCode,
                ShortUrl = $"{baseUrl.TrimEnd('/')}/{entity.ShortCode}",
                CreatedAtUtc = entity.CreatedAtUtc,
            };
        }
    }
}

