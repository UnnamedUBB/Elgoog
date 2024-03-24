using Elgoog.API.Jobs;
using Elgoog.API.Services;
using Elgoog.API.Services.Interfaces;
using Elgoog.DAL;
using Elgoog.DAL.Repositories;
using Elgoog.DAL.Repositories.Interfaces;
using Elgoog.Scrappers;
using Elgoog.Scrappers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;

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
            });
        });

        return collection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection collection)
    {
        collection.AddScoped<IProductRepository, ProductRepository>();

        return collection;
    }

    public static IServiceCollection AddServices(this IServiceCollection collection)
    {
        collection.AddScoped<IProductService, ProductService>();
        collection.AddScoped<ICeneoScrapper, CeneoScrapper>();

        return collection;
    }

    public static IServiceCollection AddJobs(this IServiceCollection collection)
    {
        collection.AddQuartz(x =>
        {
            x.AddJob<ScrapperJob>(t => t.WithIdentity(ScrapperJob.Key).Build());
            x.AddTrigger(t =>
            {
                t.WithIdentity("scrapper-trigger", "scrapper-triggers")
                    .ForJob(ScrapperJob.Key);
            });
        });

        return collection;
    }
}