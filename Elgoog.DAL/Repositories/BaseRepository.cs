using System.Linq.Expressions;
using Elgoog.DAL;
using Elgoog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Briskly.DAL.Repositories;

public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class
{
    private readonly ElgoogContext _context;
    private readonly IConfigurationProvider _configurationProvider;

    public BaseRepository(ElgoogContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    public Task<TModel> GetAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        params Expression<Func<TModel, object>>[] includes)
    {
        return PrepareQueryable(expression, orderBy, null, null, includes).FirstOrDefaultAsync();
    }

    public Task<List<TModel>> GetAllAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        params Expression<Func<TModel, object>>[] includes)
    {
        return PrepareQueryable(expression, orderBy, null, null, includes).ToListAsync();
    }

    public Task<List<TModel>> GetAllWithPaginationAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<TModel, object>>[] includes)
    {
        return PrepareQueryable(expression, orderBy, page, pageSize, includes).ToListAsync();
    }

    public Task<bool> ExistsAsync(Expression<Func<TModel, bool>> expression)
    {
        return _context.Set<TModel>().AnyAsync(expression);
    }

    public void Add(TModel model)
    {
        _context.Add(model);
    }

    public void AddRange(IEnumerable<TModel> models)
    {
        _context.AddRange(models);
    }

    public void Update(TModel model)
    {
        _context.Update(model);
    }

    public void Delete(TModel model)
    {
        _context.Remove(model);
    }
    
    private IQueryable<TModel> PrepareQueryable(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<TModel, object>>[] includes)
    {
        var baseQuery = _context.Set<TModel>()
            .AsNoTracking()
            .AsQueryable();

        if (expression is not null)
        {
            baseQuery = baseQuery.Where(expression);
        }

        if (orderBy is not null)
        {
            baseQuery = orderBy(baseQuery);
        }

        baseQuery = includes.Aggregate(baseQuery, (curr, acc) => curr.Include(acc));

        if (page is not null && pageSize is not null)
        {
            baseQuery = baseQuery.Skip(page.Value - 1 * pageSize.Value).Take(pageSize.Value);
        }

        return baseQuery;
    }
}