using Elgoog.API.Dtos;
using Elgoog.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elgoog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet]
    public async Task<OkObjectResult> GetProducts([FromQuery] ProductsRequestDto query)
    {
        var products = await _productsService.GetProductsAsync(query.Page , query.PageSize, query.Filter);
        return Ok(products);
    }
}