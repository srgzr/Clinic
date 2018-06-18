using PolyclinicProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Service.Common
{
    public abstract class CommonRepositoryAsync
    {
        public static async Task<PagingOutput<T>> GetAllWithPageAsync<T>(IQueryable<T> query, int pageNumber, int pageSize, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            query = query.Where(where);

            var offset = (pageNumber - 1) * pageSize;
            IEnumerable<T> dto = await query?.Skip(offset)?.Take(pageSize)?.ToListAsync();
            var total = await query?.CountAsync();

            return new PagingOutput<T>
            {
                Data = dto,
                TotalItems = total
            };
        }

        public static async Task<IEnumerable<T>> GetIncludeAsync<T>(IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            IEnumerable<T> dto = await query.AsNoTracking().ToListAsync();

            return dto;
        }

        public static async Task<PagingOutput<T>> GetAllWithPageAsync<T>(IQueryable<T> query, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            var offset = (pageNumber - 1) * pageSize;
            IEnumerable<T> dto = await query.Skip(offset).Take(pageSize).ToListAsync();
            var total = await query?.CountAsync();

            return new PagingOutput<T>
            {
                Data = dto,
                TotalItems = total,
                CurrentPage = pageNumber,
                ItemsPerPage = pageSize
            };
        }

        public static async Task<PagingOutput<T>> GetAllWithPageAsync<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            var offset = (pageNumber - 1) * pageSize;
            IEnumerable<T> dto = await query.OrderBy(s => s)?.Skip(offset)?.Take(pageSize)?.ToListAsync();
            var total = await query?.CountAsync();

            return new PagingOutput<T>
            {
                Data = dto,
                TotalItems = total
            };
        }
    }
}