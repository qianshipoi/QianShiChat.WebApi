namespace QianShiChat.Application.Contracts;

public class PagedList<T> where T : class
{
    public IEnumerable<T> Items { get; set; } = default!;
    public long Total { get; set; }
    public int CurrentPage { get; set; }
    public int CurrentSize { get; set; }
    public bool HasPrev { get; set; }
    public bool HasNext { get; set; }
}

public static class PagedList
{
    public static PagedList<T> Create<T>(IEnumerable<T> items, long total, int page, int size) where T : class
    {
        return new PagedList<T>
        {
            Items = items,
            Total = total,
            HasNext = page * size < total,
            HasPrev = page > 1,
            CurrentPage = page,
            CurrentSize = size
        };
    }

    public static PagedList<T> Create<T>(IEnumerable<T> items, long total, int size) where T : class
    {
        return new PagedList<T>
        {
            Items = items,
            Total = total,
            CurrentSize = items.Count(),
            CurrentPage = 0,
            HasNext = items.Count() == size,
        };
    }

    public static PagedList<T> Create<T>(IEnumerable<T> items, int size) where T : class
    {
        return new PagedList<T>
        {
            Items = items.Count() <= size ? items : items.Take(size),
            Total = 0,
            CurrentSize = items.Count(),
            CurrentPage = 0,
            HasNext = items.Count() > size,
        };
    }
}