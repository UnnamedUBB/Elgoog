namespace Elgoog.API.Dtos.Interfaces;

public interface IPageableDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}