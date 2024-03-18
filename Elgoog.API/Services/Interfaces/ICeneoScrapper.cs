namespace Elgoog.API.Services.Interfaces;

public interface ICeneoScrapper : IBaseScrapper
{
    Task<List<Product>> GetProducts(string filter);
}
