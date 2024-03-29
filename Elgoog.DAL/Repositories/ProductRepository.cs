using Elgoog.DAL.Models;
using Elgoog.DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.Extensions.Configuration;

namespace Elgoog.DAL.Repositories;

public class ProductRepository : BaseRepository<ProductModel>, IProductRepository
{
    public ProductRepository(ElgoogContext context, IConfiguration configuration) : base(context, configuration)
    {
    }

    public async Task AddOrUpdateAsync(IEnumerable<ProductModel> models)
    {
        var productModels = models.ToList();

        var ids = productModels.Select(x => x.Id);
        var updateableProducts = await GetAllAsync(x => ids.Contains(x.Id));

        var updateableIds = updateableProducts.Select(x => x.Id);
        var addableProducts = productModels.Where(x => !updateableIds.Contains(x.Id));

        foreach (var updateableProduct in updateableProducts)
        {
            var model = productModels.FirstOrDefault(x => x.Id == updateableProduct.Id);
            if (model == null) continue;

            Update(updateableProduct);
            model.Adapt(updateableProduct);
        }

        AddRange(addableProducts);
    }
}