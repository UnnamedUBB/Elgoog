using Elgoog.DAL.Models;
using Elgoog.DAL.Repositories.Interfaces;
using Elgoog.Scrappers.Interfaces;
using Quartz;

namespace Elgoog.API.Jobs;

public class ScrapperJob : IJob
{
    public static readonly JobKey Key = new JobKey("products", "scrappers");

    private readonly ICeneoScrapper _ceneoScrapper;
    private readonly IProductRepository _productRepository;

    public ScrapperJob(ICeneoScrapper ceneoScrapper, IProductRepository productRepository)
    {
        _ceneoScrapper = ceneoScrapper;
        _productRepository = productRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var filter = context.MergedJobDataMap.GetString("Filter");
        var page = context.MergedJobDataMap.GetInt("Page");

        if (filter is null)
        {
            await context.Scheduler.Shutdown();
        }
        
        try
        {
            var products = await _ceneoScrapper.GetProductsAsync(filter, page);
            var parsed = products.Select(entity => new ProductModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Price = entity.Price,
                    Image = entity.Image,
                    Reference = entity.Reference,
                })
                .ToList();

            _productRepository.AddRange(parsed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await Task.Delay(500);
    }
}