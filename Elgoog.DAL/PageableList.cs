using Elgoog.DAL.Interfaces;

namespace Elgoog.DAL;

public class PageableList<T> : IPageableList<T>
{ 
    public int TotalCount { get; set; }
    public List<T> Data { get; set; }
}