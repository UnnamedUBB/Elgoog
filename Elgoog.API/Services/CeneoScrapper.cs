using System.Text.RegularExpressions;
using Elgoog.API.Services.Interfaces;
using HtmlAgilityPack;

namespace Elgoog.API.Services;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Price { get; set; }
    public string? Link { get; set; }
    public string? Img { get; set; }
}

public sealed class CeneoScrapper : BaseScrapper, ICeneoScrapper
{
    protected override string BaseUrl => "https://www.ceneo.pl/";

    public CeneoScrapper(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<List<Product>> GetProducts(string filter)
    {
        var html = await GetPageAsync($";szukaj-{filter}");
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var productNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'cat-prod-row__body')]");
        var products = new List<Product>();
        
        foreach (var node in productNodes)
        {
            products.Add(new Product
            {
                Name = GetName(node),
                Price = GetPrice(node),
                Link = await GetReference(node),
                Img = GetImage(node)
            });    
        }

        return products;
    }

    private string GetName(HtmlNode node)
    {
        return node.SelectSingleNode(".//strong[@class='cat-prod-row__name']").InnerText.Trim();
    }

    private string GetPrice(HtmlNode node)
    {
        return node.SelectSingleNode(".//span[@class='price']").InnerText.Trim();
    }

    private string GetImage(HtmlNode node)
    {
        var img = node.SelectSingleNode(".//div[@class='cat-prod-row__foto']//img")
            .GetAttributeValue("data-original", "").Trim();
        if (img == "")
        {
            img = node.SelectSingleNode(".//div[@class='cat-prod-row__foto']//img").GetAttributeValue("src", "");
        }

        return img;
    }

    private async Task<string?> GetReference(HtmlNode node)
    {
        var link = node.SelectSingleNode(".//div[@class='btn-compare-outer']//a").GetAttributeValue("href", "").Trim();
        if (!Regex.IsMatch(link, @"^\/\d+$"))
        {
            return link;
        }

        var html = await GetPageAsync(link[1..]);
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var refNode =
            document.DocumentNode.SelectSingleNode("//a[@class='button button--primary button--flex go-to-shop']");

        var hrefAttribute = refNode?.GetAttributeValue("href", string.Empty);
        return hrefAttribute;
    }
}