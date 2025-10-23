namespace ReadingCommunityApi.Application.Dtos;

public class PageResult<T>
{
    public T Items { get; set; }
    public int TotalCount { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}