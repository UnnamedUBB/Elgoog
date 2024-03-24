using Elgoog.DAL;
using Elgoog.DAL.Models;

namespace Elgoog.API.Services.Interfaces;

public interface IProductsService
{
    Task<PageableList<ProductModel>> GetProductsAsync(int page, int pageSize, string? filter);
}