using Elgoog.API.Dtos.Interfaces;

namespace Elgoog.API.Dtos;

public class ProductsRequestDto : IPageableDto
{
    public string Filter { get; set; } = "";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public decimal MinPrice { get; set; } = 0;
    public decimal MaxPrice { get; set; } = 99999999;
    public SortType SortType { get; set; } = SortType.LowestPrice;
}