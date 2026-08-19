using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Application.DTOs
{
    public class CreateShortUrlRequestDto
    {
        [Required, Url]
        public string OriginalUrl { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CustomCode { get; set; }
    }

    public class ShortUrlResponseDto
    {
        public string ShortCode { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }
}

