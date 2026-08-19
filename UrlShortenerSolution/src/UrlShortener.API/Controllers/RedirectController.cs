using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.API.Controllers
{

    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IUrlShortenerService _urlService;

        public RedirectController(IUrlShortenerService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("/{shortCode}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var result = await _urlService.ResolveAsync(shortCode);

            if (!result.Success)
            {
                return NotFound(new { error = result.Error });
            }

            return Redirect(result.Data!);
        }
    }
}
