using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Food.Core.Requests
{
    public class PaginatedRequest<TResultItem> : IRequest<PaginatedResult<TResultItem>>
    {
        public int Page { get; set; } = 1;

        public int Count { get; set; } = 10;

    }

    public class PaginatedResult<T>
    {
        public List<T> Items { get; private set; }
        
        public int TotalCount { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }
        
        public PaginatedResult(List<T> items, int count, int currentPage, int pageSize)
        {
            Items = items;
            TotalCount = count;
            CurrentPage = currentPage;
            PageSize = pageSize; 
            TotalPages =  (int)Math.Ceiling(count / (double)pageSize);
        }
        
        public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<T>(items, count, currentPage, pageSize);
        }
    }

    public static class PaginatedResultExt
    {
        public static Task<PaginatedResult<T>> CreatePaginatedResultAsync<T>(this IQueryable<T> source, int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            return PaginatedResult<T>.CreateAsync(source, currentPage, pageSize, cancellationToken);
        }

        public static Task<PaginatedResult<T>> CreatePaginatedResultAsync<T>(this IQueryable<T> source, PaginatedRequest<T> request, CancellationToken cancellationToken = default)
        {
            return PaginatedResult<T>.CreateAsync(source, request.Page, request.Count, cancellationToken);
        }
    }
}