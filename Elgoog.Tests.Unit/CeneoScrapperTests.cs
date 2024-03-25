using Elgoog.Scrappers;
using Elgoog.Scrappers.Dto;
using Elgoog.Tests.Shared;
using HtmlAgilityPack;
using Moq;
using Xunit.Sdk;

namespace Elgoog.Tests.Unit;

public class CeneoScrapperTests
{
    [Fact]
    public void GetName_Should_Returns_Null()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<span></span>");

        // Act
        var result = TestHelper.InvokePrivateMethod(typeof(CeneoScrapper), "GetName", doc.DocumentNode);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetName_Should_Returns_Name()
    {
        // Arrange
        var html = "<strong class='cat-prod-row__name'>Product Name</strong>";
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Act
        var result = TestHelper.InvokePrivateMethod(typeof(CeneoScrapper), "GetName", doc.DocumentNode);

        // Assert
        Assert.Equal("Product Name", result);
    }

    [Fact]
    public void GetPrice_Should_Returns_Null()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<span></span>");

        // Act
        var result = TestHelper.InvokePrivateMethod(typeof(CeneoScrapper), "GetPrice", doc.DocumentNode);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetName_Should_Returns_Price()
    {
        // Arrange
        var html = "<span class=\"price\"><span class=\"value\">99</span><span class=\"penny\">,00</span></span>";
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Act
        var result = TestHelper.InvokePrivateMethod(typeof(CeneoScrapper), "GetPrice", doc.DocumentNode);

        // Assert
        Assert.Equal(99.00m, result);
    }

    [Fact]
    public void GetImage_Should_Returns_Null()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<span></span>");

        // Act
        var result = TestHelper.InvokePrivateMethod(typeof(CeneoScrapper), "GetImage", doc.DocumentNode);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public void GetImage_Should_Returns_Image_Url()
    {
        // Arrange
        var html =
            "<div class=\"cat-prod-row__foto\">\n                <a href=\"/148898746\" class=\"js_clickHash js_seoUrl product-link go-to-product\" title=\"Indiana Mtb X Pulser 1.6 Męski Czarno Czerwony 26 2023\">\n                        <img src=\"//image.ceneostatic.pl/data/products/148898746/f-indiana-mtb-x-pulser-1-6-meski-czarno-czerwony-26-2023.jpg\" alt=\"Indiana Mtb X Pulser 1.6 Męski Czarno Czerwony 26 2023\">\n                                                        </a>\n\n                \n\n            </div>";
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Act
        var result = TestHelper.InvokePrivateMethod(typeof(CeneoScrapper), "GetImage", doc.DocumentNode);
        Console.WriteLine(result);
        
        // Assert
        Assert.Equal("www.ceneo.pl//image.ceneostatic.pl/data/products/148898746/f-indiana-mtb-x-pulser-1-6-meski-czarno-czerwony-26-2023.jpg", result);
    }
}