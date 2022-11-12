﻿namespace QianShiChat.WebApi.Models
{
    public class PagedList<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }

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
    }
}
