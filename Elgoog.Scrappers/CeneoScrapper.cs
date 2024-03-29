using System.Text.RegularExpressions;
using Elgoog.Scrappers.Dto;
using Elgoog.Scrappers.Interfaces;
using HtmlAgilityPack;

namespace Elgoog.Scrappers;

public sealed class CeneoScrapper : BaseScrapper, ICeneoScrapper
{
    protected override string BaseUrl => "https://www.ceneo.pl/";

    public async Task<ProductDto?> GetProductAsync(int id)
    {
        var document = await GetPageAsync($"/{id}");
        if (document == null) return null;
        
        var name = GetName(document.DocumentNode);
        var price = GetPrice(document.DocumentNode);
        var link = await GetReference(document.DocumentNode).ConfigureAwait(false);
        var image = GetImage(document.DocumentNode);
        
        if (name == null || price == null || link == null || image == null)
            return null;

        return new ProductDto
        {
            Id = id,
            Name = name,
            Price = (decimal)price,
            Reference = link,
            Image = image
        };
    }
    
    public async Task<List<ProductDto>> GetProductsAsync(string filter, int page = 0)
    {
        var document = await GetPageAsync($";szukaj-{filter};0020-30-0-0-{page}.htm").ConfigureAwait(false);
        if (document == null) return [];

        var productNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'cat-prod-row')]");
        if (productNodes == null) return [];

        var products = new List<ProductDto>();
        Parallel.ForEach(productNodes, async (node) =>
        {
            var id = node.GetAttributeValue("data-pid", null);
            var name = GetName(node);
            var price = GetPrice(node);
            var link = await GetReference(node).ConfigureAwait(false);
            var image = GetImage(node);

            if (id == null || name == null || price == null || link == null || image == null)
                return;
            
            if (!int.TryParse(id, out var parsedId))
                return;

            products.Add(new ProductDto
            {
                Id = parsedId,
                Name = name,
                Price = (decimal) price,
                Reference = link,
                Image = image
            });
        });

        return products.DistinctBy(x => x.Id).ToList();
    }

    private string? GetName(HtmlNode node)
    {
        return node.SelectSingleNode(".//strong[@class='cat-prod-row__name']")?.InnerText.Trim();
    }

    private decimal? GetPrice(HtmlNode node)
    {
        var priceNode = node.SelectSingleNode(".//span[@class='price']");
        if (priceNode == null) return null;

        var price = priceNode.InnerText.Trim();
        var preparedPrice = Regex.Replace(price, @"\s+", "").Replace(",", "."); 
        
        if (decimal.TryParse(preparedPrice, out var parsed))
            return parsed;
        
        return null;
    }

    private string? GetImage(HtmlNode node)
    {
        var img = node.SelectSingleNode(".//div[@class='cat-prod-row__foto']//img")?
            .GetAttributeValue("data-original", "").Trim();
        
        if (img == "")
        {
            img = node.SelectSingleNode(".//div[@class='cat-prod-row__foto']//img").GetAttributeValue("src", "");
        }
        
        return img;
    }

    private async Task<string?> GetReference(HtmlNode node)
    {
        var linkNode = node.SelectSingleNode(".//div[@class='btn-compare-outer']//a");
        if (linkNode == null) return null;
        
        var link = linkNode.GetAttributeValue("href", "").Trim();
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