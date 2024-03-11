using Elgoog.API.Services;
using Elgoog.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elgoog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ICeneoScrapper _ceneoScrapper;

    public TestController(ICeneoScrapper ceneoScrapper)
    {
        _ceneoScrapper = ceneoScrapper;
    }

    [HttpGet]
    public async Task<OkObjectResult> GetProducts(string filter)
    {
        var data = await _ceneoScrapper.GetProducts(filter);
        
        return Ok(data);
    }
}