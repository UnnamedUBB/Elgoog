using Elgoog.API.Dtos.Interfaces;

namespace Elgoog.API.Dtos;

public class ProductsRequestDto : IPageableDto
{
    public string? Filter { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}