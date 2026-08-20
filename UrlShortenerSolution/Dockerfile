# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore UrlShortener.sln
RUN dotnet publish src/UrlShortener.API/UrlShortener.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV DOTNET_hostBuilder__reloadConfigOnChange=false

CMD ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet UrlShortener.API.dll"]