using Elgoog.DAL;
using Elgoog.DAL.Models;

namespace Elgoog.API.Services.Interfaces;

public interface IProductService
{
    Task<PageableList<ProductModel>> GetProductsAsync(int page, int pageSize, string? filter);
}