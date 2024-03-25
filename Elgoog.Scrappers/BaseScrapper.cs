using Elgoog.Scrappers.Interfaces;
using HtmlAgilityPack;

namespace Elgoog.Scrappers;

public abstract class BaseScrapper : IBaseScrapper
{
    protected abstract string BaseUrl
    {
        get;
    }
    
    public async Task<HtmlDocument?> GetPageAsync(string url)
    {
        try
        {
            if (BaseUrl == null)
            {
                throw new Exception("Nieprawdiłowy base url");
            }
            
            var web = new HtmlWeb();
            return web.Load(BaseUrl + url);
        }
        catch (HttpRequestException e)
        {
            return null;
        }
    }
}