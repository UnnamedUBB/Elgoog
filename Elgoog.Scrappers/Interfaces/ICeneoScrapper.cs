using Elgoog.Scrappers.Dto;

namespace Elgoog.Scrappers.Interfaces;

public interface ICeneoScrapper : IBaseScrapper
{
    Task<List<ProductDto>> GetProductsAsync(string filter, int page = 0);
}