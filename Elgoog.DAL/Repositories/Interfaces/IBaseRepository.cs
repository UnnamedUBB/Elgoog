using System.Linq.Expressions;

namespace Elgoog.DAL.Repositories.Interfaces;

public interface IBaseRepository<TModel> where TModel : class
{
    public Task<TModel> GetAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        params Expression<Func<TModel, object>>[] includes);

    public Task<List<TModel>> GetAllAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        params Expression<Func<TModel, object>>[] includes);

    public Task<List<TModel>> GetAllWithPaginationAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<TModel, object>>[] includes);

    public Task<bool> ExistsAsync(Expression<Func<TModel, bool>> expression);

    public void Add(TModel model);
    public void AddRange(IEnumerable<TModel> models);
    public void Update(TModel model);
    public void Delete(TModel model);
}