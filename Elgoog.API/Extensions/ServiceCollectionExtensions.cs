using Elgoog.API.Services;
using Elgoog.API.Services.Interfaces;
using Elgoog.DAL;
using Elgoog.DAL.Repositories;
using Elgoog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elgoog.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElgoogContext(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddDbContext<ElgoogContext>(x =>
        {
            var connectionString = configuration.GetConnectionString("elgoog");
            x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), t =>
            {
                t.MigrationsAssembly("Elgoog.DAL");
                t.EnableRetryOnFailure();
            });
        });

        return collection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection collection)
    {
        collection.AddScoped<IItemRepository, ItemRepository>();
        
        return collection;
    }

    public static IServiceCollection AddScrappers(this IServiceCollection collection)
    {
        collection.AddScoped<ICeneoScrapper, CeneoScrapper>();
        
        return collection;
    }
}