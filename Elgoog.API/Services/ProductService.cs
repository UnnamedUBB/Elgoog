using Elgoog.API.Services.Interfaces;
using Elgoog.DAL;
using Elgoog.DAL.Models;
using Elgoog.DAL.Repositories.Interfaces;
using Elgoog.Scrappers.Interfaces;
using Quartz.Util;

namespace Elgoog.API.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICeneoScrapper _ceneoScrapper;

    public ProductService(IProductRepository productRepository, ICeneoScrapper ceneoScrapper)
    {
        _productRepository = productRepository;
        _ceneoScrapper = ceneoScrapper;
    }

    public async Task<PageableList<ProductModel>> GetProductsAsync(int page, int pageSize, string? filter)
    {
        var result = await _productRepository.GetAllWithPaginationAsync(
            x => !filter.IsNullOrWhiteSpace() && x.Name.Contains(filter!), null, page, pageSize);

        if (result.Data.Count == 0 && !filter.IsNullOrWhiteSpace())
        {
            var scrappedData = await _ceneoScrapper.GetProductsAsync(filter!, 0);
            var preparedScrappedData = scrappedData.Select(x => new ProductModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Image = x.Image,
                Reference = x.Reference,
                DateModifiedUtc = DateTime.UtcNow
            }).ToList();
            
            await _productRepository.AddOrUpdateAsync(preparedScrappedData);
            await _productRepository.SaveAsync();

            result.Data = preparedScrappedData[..Math.Min(preparedScrappedData.Count(), 19)];
            result.TotalCount = Math.Min(preparedScrappedData.Count(), 20);
        }

        return result;
    }
}