using HtmlAgilityPack;

namespace Elgoog.Scrappers.Interfaces;

public interface IBaseScrapper
{
    Task<HtmlDocument?> GetPageAsync(string url);
}