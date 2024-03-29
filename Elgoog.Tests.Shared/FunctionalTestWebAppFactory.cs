using Elgoog.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MySql;
using Xunit;

namespace Elgoog.Tests.Shared;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _dbContainer = new MySqlBuilder()
        .WithImage("mariadb")
        .WithUsername("root")
        .WithPassword("root")
        .WithDatabase("elgoog")
        .Build();
    
    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(x =>
        {
            var serviceDescriptor = typeof(ElgoogContext);
            if (x.FirstOrDefault(t => t.ServiceType == serviceDescriptor) is {} descriptor)
            {
                x.Remove(descriptor);
            }

            x.AddDbContext<ElgoogContext>(t =>
            {
                var connectionString = _dbContainer.GetConnectionString();
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                
                t.UseMySql(connectionString, serverVersion);
            });
        });
    }
}