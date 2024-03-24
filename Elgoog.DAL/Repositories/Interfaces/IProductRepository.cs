using Elgoog.DAL.Models;

namespace Elgoog.DAL.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<ProductModel>
{
    Task AddOrUpdateAsync(IEnumerable<ProductModel> models);
}