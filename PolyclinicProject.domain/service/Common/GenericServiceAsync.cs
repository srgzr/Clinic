using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Service.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Service
{
    public abstract class GenericServiceAsync<T> : IServiceAsync<T>
        where T : class, new()
    {
        #region Setting

        public readonly IDbContext _context;
        private IDbSet<T> _entities;
        protected readonly DbContext _commonContext;
        protected DbSet<T> DbSet => _commonContext.Set<T>();
        protected DbQuery<T> DbSetNoTrack => _commonContext.Set<T>().AsNoTracking();

        #endregion Setting

        protected GenericServiceAsync(IDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        protected virtual IDbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());
        public virtual IQueryable<T> Table => Entities;

        /// <summary>
        /// получение списка всех объектов
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        /// <summary>
        /// поиск по Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// поиск 1 значения
        /// </summary>
        /// <param name="match">параметры</param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _entities.SingleOrDefaultAsync(match);
        }

        /// <summary>
        /// поиск всех значений
        /// </summary>
        /// <param name="match">параметры</param>
        /// <returns></returns>
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _entities.Where(match).ToListAsync();
        }

        /// <summary>
        /// реализация добавления
        /// </summary>
        /// <param name="entity"></param>
        public async Task<T> AddAsync(T entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// добавление выбранных значений
        /// </summary>
        /// <param name="entity"></param>
        public async virtual Task AddAllAsync(IEnumerable<T> entity)
        {
            foreach (var ent in entity)
            {
                var entry = _context.Entry(ent);
                entry.State = EntityState.Added;
                _entities.Add(ent);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// реализация редактирования
        /// </summary>
        /// <param name="entity"></param>
        public async Task<T> UpdateAsync(T updated, int key)
        {
            if (updated == null)
                return null;
            // T existing = await _context.Set<T>().FindAsync(key);
            //if (existing != null)

            var local = await _context.Set<T>().FindAsync(key);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            var entry = _context.Entry(updated);
            entry.State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return updated;
        }

        /// <summary>
        /// реализация удаления
        /// </summary>
        /// <param name="id"></param>
        public async Task<int> DeleteAsync(T t)
        {
            _entities.Remove(t);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// реализация удаления
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteAsync(int id)
        {
            var t = await _context.Set<T>().FindAsync(id);
            if (t != null)
            {
                _entities.Remove(t);
                await _context.SaveChangesAsync();
            }
            else
                throw new ArgumentNullException("entity");
        }

        public async virtual Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
            }
        }

        /// <summary>
        /// получение данных по страницам
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagingOutput<T>> GetAllAsync(int page, int pageSize)
        {
            return await CommonRepositoryAsync.GetAllWithPageAsync(_entities.OrderBy(s => s), page, pageSize);
        }

        /// <summary>
        /// получение данных с вклбченными элементами
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IQueryable<T> GetAllWithInclude(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entities;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }

        /// <summary>
        /// получение данных с вклбченными элементами
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<T> GetWithIncludeAsync<T>(int id, params Expression<Func<T, object>>[] includes) where T : CommanEntity
        {
            var query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var incl in includes)
                    _context.Set<T>().Include(incl);
            }

            return query.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}