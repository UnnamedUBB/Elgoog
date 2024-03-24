using System.Linq.Expressions;
using Elgoog.DAL.Interfaces;
using Elgoog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Elgoog.DAL.Repositories;

public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class
{
    private readonly ElgoogContext _context;
    private readonly IConfiguration _configuration;

    public BaseRepository(ElgoogContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public Task<TModel?> GetAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
        params Expression<Func<TModel, object>>[] includes)
    {
        return PrepareQueryable(expression, orderBy, includes).FirstOrDefaultAsync();
    }

    public Task<List<TModel>> GetAllAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
        params Expression<Func<TModel, object>>[] includes)
    {
        return PrepareQueryable(expression, orderBy, includes).ToListAsync();
    }

    public async Task<PageableList<TModel>> GetAllWithPaginationAsync(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
        int? page = null,
        int? pageSize = null,
        params Expression<Func<TModel, object>>[] includes)
    {
        var query = PrepareQueryable(expression, orderBy,  includes);

        var total = await query.CountAsync();
        
        if (page is not null && pageSize is not null)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return new PageableList<TModel>
        {
            TotalCount = total,
            Data = await query.ToListAsync()
        };
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

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public IQueryable<TModel> PrepareQueryable(Expression<Func<TModel, bool>> expression = null,
        Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
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

        return baseQuery;
    }
}