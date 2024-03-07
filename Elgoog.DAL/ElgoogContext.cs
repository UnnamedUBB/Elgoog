using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Elgoog.DAL;

public class ElgoogContext : DbContext
{
    public ElgoogContext(DbContextOptions<ElgoogContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElgoogContext).Assembly);

        var types = typeof(ElgoogContext).Assembly
            .GetTypes()
            .Where(x => x.GetCustomAttribute<TableAttribute>() is not null)
            .ToList();

        foreach (var type in types)
        {
            var tableName = type.GetCustomAttribute<TableAttribute>()!.Name;
            var model = modelBuilder.Entity(type).ToTable(tableName);
        }
    }
}