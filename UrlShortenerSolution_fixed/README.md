# URL Shortener API (C#, layered architecture, no login)

A public URL shortener backend — anyone can shorten a link, no account needed.

## Layers

```
UrlShortener.API              <- Controllers, Program.cs, Swagger
      |
      v
UrlShortener.Infrastructure    <- EF Core DbContext, UrlRepository (SQLite)
      |
      v
UrlShortener.Application       <- UrlShortenerService, DTOs, interfaces
      |
      v
UrlShortener.Domain             <- ShortenedUrl entity only
```

```
src/
  UrlShortener.Domain/
    Entities/ShortenedUrl.cs        – Id, OriginalUrl, ShortCode, CreatedAtUtc
  UrlShortener.Application/
    DTOs/UrlDtos.cs                  – CreateShortUrlRequestDto, ShortUrlResponseDto
    Interfaces/IUrlRepository.cs, IUrlShortenerService.cs
    Services/UrlShortenerService.cs  – generates/validates short codes, resolves them
    Common/ServiceResult.cs
  UrlShortener.Infrastructure/
    Persistence/AppDbContext.cs
    Repositories/UrlRepository.cs
    DependencyInjection.cs           – single AddInfrastructure() call wires it all up
  UrlShortener.API/
    Controllers/UrlsController.cs    – POST /api/urls (create)
    Controllers/RedirectController.cs – GET /{shortCode} (public redirect)
    Program.cs
    appsettings.json
```

There is intentionally **no auth**: no `User` entity, no JWT, no `[Authorize]`
anywhere. Every endpoint is open.

## Endpoints

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/urls` | Create a shortened URL. Body: `{ "originalUrl": "...", "customCode": "..." }` (`customCode` optional). Returns `{ shortCode, shortUrl, createdAtUtc }`. |
| `GET` | `/{shortCode}` | 302-redirects to the original URL. |

## Running it

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
cd src/UrlShortener.API
dotnet restore
dotnet run
```

SQLite (`urlshortener.db`) is created automatically on first run. Open
`https://localhost:5001/swagger` (or `http://localhost:5000/swagger`) to try
it directly.

## Wiring up the frontend

The companion React frontend (`url-shortener-frontend/`) expects exactly this
response shape from `POST /api/urls`:

```json
{ "shortCode": "aB3kZ1", "shortUrl": "https://localhost:5001/aB3kZ1", "createdAtUtc": "2026-08-17T20:10:00Z" }
```

To connect them:

1. In the frontend, copy `.env.example` to `.env` and set:
   ```
   VITE_API_URL=https://localhost:5001/api
   ```
   (or `http://localhost:5000/api` if you're running the `http` launch profile).
2. Run the backend (`dotnet run` from `src/UrlShortener.API`) and the
   frontend (`npm run dev`) side by side.
3. CORS is already wide open (`AllowAnyOrigin`) in `Program.cs`, so the
   frontend's dev server (`http://localhost:5173`) can call the API directly
   with no further config. Tighten this to a specific origin before
   deploying either side publicly.

## Notes

- `EnsureCreated()` is used for local dev convenience. Switch to EF Core
  migrations (`dotnet ef migrations add InitialCreate`) before deploying.
- Because there's no auth, a shortened URL isn't tied to anyone — the
  `ShortenedUrl` entity is just `Id`, `OriginalUrl`, `ShortCode`,
  `CreatedAtUtc`. If you want click tracking, ownership, or expiry back later,
  those fields can be re-added to the entity + DTOs + service.
