namespace Elgoog.DAL.Interfaces;

public interface IPageableList<T>
{
    int TotalCount { get; set; }
    List<T> Data { get; set; }
}