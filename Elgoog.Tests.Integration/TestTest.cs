using Elgoog.Tests.Shared;

namespace Elgoog.Tests.Integration;

public class TestTest : BaseIntegrationTest
{
    [Fact]
    public void test()
    {
        Assert.Equal(1, 1);
    }

    public TestTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }
}