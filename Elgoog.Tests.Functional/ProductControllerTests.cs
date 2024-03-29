using System.Net;
using Elgoog.DAL;
using Elgoog.DAL.Models;
using Elgoog.Tests.Shared;
using FluentAssertions;

namespace Elgoog.Tests.Functional;

public class ProductControllerTests : BaseFunctionalTest
{
    public ProductControllerTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_Returns_List_Of_Products()
    {
        var response = await HttpClient.GetAsync("Products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // response.Content.Should().BeAssignableTo<PageableList<ProductModel>>();
    }
}