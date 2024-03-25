namespace Elgoog.Tests.Shared;

public abstract class BaseFunctionalTest : BaseIntegrationTest, IDisposable
{
    protected readonly HttpClient HttpClient;
    
    public BaseFunctionalTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
        HttpClient = factory.CreateClient();
    }

    public new void Dispose()
    {
        HttpClient.Dispose();
        base.Dispose();
    }
}