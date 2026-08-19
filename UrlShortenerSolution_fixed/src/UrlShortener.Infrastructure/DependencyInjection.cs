using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;
using UrlShortener.Infrastructure.Persistence;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.Infrastructure
{
    /// <summary>
    /// Wires up the Infrastructure and Application layers so the API project
    /// only needs a single call in Program.cs, keeping composition-root
    /// knowledge out of the presentation layer.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=urlshortener.db";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            // Repositories
            services.AddScoped<IUrlRepository, UrlRepository>();

            // Application services (business logic)
            services.AddScoped<IUrlShortenerService, UrlShortenerService>();

            return services;
        }
    }
}

