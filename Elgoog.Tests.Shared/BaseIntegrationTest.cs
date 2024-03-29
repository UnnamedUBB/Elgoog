using Elgoog.DAL;
 using Microsoft.Extensions.DependencyInjection;
 using Xunit;
 
 namespace Elgoog.Tests.Shared;
 
 public abstract class BaseIntegrationTest : IClassFixture<FunctionalTestWebAppFactory>, IDisposable
 {
     private readonly IServiceScope _scope;
     protected readonly ElgoogContext Context;
     
     protected BaseIntegrationTest(FunctionalTestWebAppFactory factory)
     {
         _scope= factory.Services.CreateScope();
         Context = _scope.ServiceProvider.GetRequiredService<ElgoogContext>();
     }
 
     public void Dispose()
     {
         Context.Dispose();
         _scope.Dispose();
     }
 }