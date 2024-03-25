using Elgoog.DAL.Repositories.Interfaces;
using Elgoog.Scrappers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Elgoog.API.Jobs;

public class ProductsJob : IJob
{
    public static readonly JobKey Key = new JobKey("products", "scrappers");

    private readonly ICeneoScrapper _ceneoScrapper;
    private readonly IProductRepository _productRepository;

    public ProductsJob(ICeneoScrapper ceneoScrapper, IProductRepository productRepository)
    {
        _ceneoScrapper = ceneoScrapper;
        _productRepository = productRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentPage = 1;

        do
        {
            var data = await _productRepository.GetAllWithPaginationAsync(
                x => EF.Functions.DateDiffDay(x.DateModifiedUtc, DateTime.UtcNow) >= 1, null, currentPage, 10);

            Parallel.ForEach(data.Data, async (product) =>
            {
                var updatedData = await _ceneoScrapper.GetProductAsync(product.Id);
                if (updatedData == null) return;
                
                _productRepository.Update(product);
                product.Reference = updatedData.Reference;
                product.Price = updatedData.Price;
                product.Name = updatedData.Name;
                product.DateModifiedUtc = DateTime.UtcNow;
            });

            currentPage++;
            
            if (data.TotalCount / 10 < currentPage + 1)
            {
                break;
            }
            
            await Task.Delay(5000);
        } while (true);
    }
}