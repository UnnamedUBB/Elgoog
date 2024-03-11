using System.Linq.Expressions;

namespace Elgoog.DAL.Repositories.Interfaces;

public interface IBaseRepository<TModel> where TModel : class
{
    Task<TModel> GetAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        params Expression<Func<TModel, object>>[] includes);

    Task<List<TModel>> GetAllAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        params Expression<Func<TModel, object>>[] includes);

    Task<List<TModel>> GetAllWithPaginationAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<TModel, object>>[] includes);

    Task<bool> ExistsAsync(Expression<Func<TModel, bool>> expression);

    void Add(TModel model);
    void AddRange(IEnumerable<TModel> models);
    void Update(TModel model);
    void Delete(TModel model);
}