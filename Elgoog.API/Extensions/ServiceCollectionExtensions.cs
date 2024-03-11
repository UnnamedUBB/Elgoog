using Elgoog.DAL;
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
}