using Elgoog.API.Services.Interfaces;
using HtmlAgilityPack;

namespace Elgoog.API.Services;

public abstract class BaseScrapper : IBaseScrapper
{
    private readonly HttpClient _httpClient;

    protected virtual string BaseUrl { get; set; } = "";

    public BaseScrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> GetPageAsync(string url)
    {
        try
        {
            if (BaseUrl == null)
            {
                throw new Exception("Nieprawdiłowy base url");
            }
            
            return await _httpClient.GetStringAsync(BaseUrl + url);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Błąd podczas pobierania strony: {e.Message}");
            return string.Empty;
        }
    }
}