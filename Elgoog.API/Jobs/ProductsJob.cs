using Elgoog.DAL.Models;
using Elgoog.DAL.Repositories.Interfaces;
using Elgoog.Scrappers.Interfaces;
using Quartz;

namespace Elgoog.API.Jobs;

public class UpdateProductsJob : IJob
{
    public static readonly JobKey Key = new JobKey("products", "scrappers");

    private readonly ICeneoScrapper _ceneoScrapper;
    private readonly IProductRepository _productRepository;

    public UpdateProductsJob(ICeneoScrapper ceneoScrapper, IProductRepository productRepository)
    {
        _ceneoScrapper = ceneoScrapper;
        _productRepository = productRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Work");

        await Task.Delay(500);
    }
}