using System.Text.RegularExpressions;
using Elgoog.Scrappers.Dto;
using Elgoog.Scrappers.Interfaces;
using HtmlAgilityPack;

namespace Elgoog.Scrappers;

public sealed class CeneoScrapper : BaseScrapper, ICeneoScrapper
{
    protected override string BaseUrl => "https://www.ceneo.pl/";

    public async Task<List<ProductDto>> GetProductsAsync(string filter, int page = 0)
    {
        var document = await GetPageAsync($";szukaj-{filter};0020-30-0-0-{page}.htm").ConfigureAwait(false);
        if (document == null) return [];

        var productNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'cat-prod-row')]");
        var products = new List<ProductDto>();
        
        Parallel.ForEach(productNodes, async (node) =>
        {
            if (!int.TryParse(node.GetAttributeValue("data-pid", ""), out var id))
                return;
            
            products.Add(new ProductDto
            { 
                Id = id,
                Name = GetName(node),
                Price = GetPrice(node),
                Link = await GetReference(node).ConfigureAwait(false) ?? "",
                Img = GetImage(node)
            });
        });

        return products;
    }

    private string GetName(HtmlNode node)
    {
        return node.SelectSingleNode(".//strong[@class='cat-prod-row__name']").InnerText.Trim();
    }

    private decimal GetPrice(HtmlNode node)
    {
        var price = node.SelectSingleNode(".//span[@class='price']").InnerText.Trim();
        var preparedPrice = Regex.Replace(price, @"\s+", "").Replace(',', '.');

        if (decimal.TryParse(preparedPrice, out var parsed))
            return parsed;

        return 0;
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

        var document = await GetPageAsync(link[1..]).ConfigureAwait(false);
        if (document == null) return null;

        var refNode =
            document.DocumentNode.SelectSingleNode("//a[@class='button button--primary button--flex go-to-shop']");

        var hrefAttribute = refNode?.GetAttributeValue("href", string.Empty);
        return hrefAttribute;
    }
}