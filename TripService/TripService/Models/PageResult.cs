namespace TripService.Models;

public class PageResult<T>
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<T> Data { get; set; } = new();
}