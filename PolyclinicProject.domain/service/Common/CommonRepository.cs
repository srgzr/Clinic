using PolyclinicProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PolyclinicProject.Domain.Service.Common
{
    public abstract class CommonRepository : CommonRepositoryAsync
    {
        public virtual PagingOutput<T> GetAllWithPage<T>(IQueryable<T> query, int pageNumber, int pageSize, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            query = query.Where(where);

            var offset = (pageNumber - 1) * pageSize;
            IEnumerable<T> dto = query?.Skip(offset)?.Take(pageSize)?.ToList();
            var total = query?.Count() ?? 0;

            return new PagingOutput<T>
            {
                Data = dto,
                TotalItems = total
            };
        }

        public virtual PagingOutput<T> GetAllWithPage<T>(IQueryable<T> query, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            var offset = (pageNumber - 1) * pageSize;
            IEnumerable<T> dto = query?.Skip(offset)?.Take(pageSize)?.ToList();
            var total = query?.Count() ?? 0;

            return new PagingOutput<T>
            {
                Data = dto,
                TotalItems = total
            };
        }

        public virtual PagingOutput<T> GetAllWithPage<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            var offset = (pageNumber - 1) * pageSize;
            IEnumerable<T> dto = query?.Skip(offset)?.Take(pageSize)?.ToList();
            var total = query?.Count() ?? 0;

            return new PagingOutput<T>
            {
                Data = dto,
                TotalItems = total
            };
        }
    }
}