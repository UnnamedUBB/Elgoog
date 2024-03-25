using Elgoog.API.Dtos;
using Elgoog.API.Services.Interfaces;
using Elgoog.DAL;
using Elgoog.DAL.Models;
using Elgoog.DAL.Repositories.Interfaces;
using Elgoog.Scrappers.Interfaces;
using Microsoft.EntityFrameworkCore;
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

    public async Task<PageableList<ProductModel>> GetProductsAsync(int page, int pageSize, decimal minPrice, decimal maxPrice,
        SortType sortType, string filter)
    {
        var result = await _productRepository.GetAllWithPaginationAsync(
            x => !filter.IsNullOrWhiteSpace() && x.Name.Contains(filter!) && minPrice <= x.Price && maxPrice >= x.Price, 
            x => sortType == SortType.LowestPrice ? x.OrderBy(t => t.Price) : x.OrderByDescending(t => t.Price), 
            page, 
            pageSize
            );

        if (result.TotalCount == 0 && !filter.IsNullOrWhiteSpace())
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

            if (sortType == SortType.LowestPrice)
            {
                var sortedData = preparedScrappedData.OrderBy(x => x.Price).ToList();
                result.Data = sortedData[..Math.Min(preparedScrappedData.Count(), 19)];
            }
            else
            {
                var sortedData = preparedScrappedData.OrderByDescending(x => x.Price).ToList();
                result.Data = sortedData[..Math.Min(preparedScrappedData.Count(), 19)];
            }
            result.TotalCount = Math.Min(preparedScrappedData.Count(), 20);
        }

        return result;
    }
}