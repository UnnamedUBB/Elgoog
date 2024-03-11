namespace Elgoog.API.Services.Interfaces;

public interface IBaseScrapper
{
    Task<string> GetPageAsync(string url);
}