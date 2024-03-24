using Elgoog.API.Dtos;
using Elgoog.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elgoog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<OkObjectResult> GetProducts([FromQuery] ProductsRequestDto query)
    {
        var products = await _productService.GetProductsAsync(query.Page , query.PageSize, query.Filter);
        return Ok(products);
    }
}