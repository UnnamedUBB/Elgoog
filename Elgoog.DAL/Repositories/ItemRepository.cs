using Elgoog.DAL.Models;
using Elgoog.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Elgoog.DAL.Repositories;

public class ItemRepository : BaseRepository<ItemModel>, IItemRepository
{
    public ItemRepository(ElgoogContext context, IConfiguration configuration) : base(context, configuration)
    {
    }
}