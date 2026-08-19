using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.API.Controllers
{

    [ApiController]
    [Route("api/urls")]
    public class UrlsController : ControllerBase
    {
        private readonly IUrlShortenerService _urlService;
        private readonly IConfiguration _configuration;

        public UrlsController(IUrlShortenerService urlService, IConfiguration configuration)
        {
            _urlService = urlService;
            _configuration = configuration;
        }



        [HttpPost]
        [ProducesResponseType(typeof(ShortUrlResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baseUrl = GetBaseUrl();

            var result = await _urlService.CreateShortUrlAsync(request, baseUrl);

            if (!result.Success)
            {
                return BadRequest(new { error = result.Error });
            }

            return StatusCode(StatusCodes.Status201Created, result.Data);
        }

        private string GetBaseUrl()
        {
            var configured = _configuration["AppBaseUrl"];
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured;
            }
            return $"{Request.Scheme}://{Request.Host}";
        }
    }
}
