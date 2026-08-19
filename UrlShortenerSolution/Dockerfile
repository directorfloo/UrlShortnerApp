# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore/publish just the API project
# (project references pull in Application/Infrastructure/Domain automatically)
COPY . .
RUN dotnet restore UrlShortener.sln
RUN dotnet publish src/UrlShortener.API/UrlShortener.API.csproj -c Release -o /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render (and most PaaS hosts) inject a $PORT env var at runtime and expect
# the app to listen on it. ASPNETCORE_URLS is set at container start (not
# baked in at build time) so it always matches whatever port is assigned.
CMD ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet UrlShortener.API.dll"]
