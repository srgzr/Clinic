using PolyclinicProject.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace PolyclinicProject.Domain.Service.Common
{
    public abstract class GenericService<T> : GenericServiceAsync<T>, IService<T>
        where T : class, new()
        //<TDto> where TDto : class, new()
    {
        public readonly new IDbContext Context;
        public IDbSet<T> _entities;
        public readonly new DbContext _commonContext;
        protected new DbSet<T> DbSet => _commonContext.Set<T>();
        protected new DbQuery<T> DbSetNoTrack => _commonContext.Set<T>().AsNoTracking();

        public GenericService(IDbContext context) : base(context)
        {
            Context = context;
            _entities = Context.Set<T>();
        }

        protected virtual new IDbSet<T> Entities => _entities ?? (_entities = Context.Set<T>());

        public virtual new IQueryable<T> Table => Entities;

        /// <summary>
        /// получение списка всех объектов
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _entities;
        }

        public T GetById(int id)
        {
            return _entities.Find(id);
        }

        /// <summary>
        /// реализация поиска
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Where(predicate);
            return query;
        }

        /// <summary>
        /// реализация добавления
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _entities.Add(entity);
        }

        /// <summary>
        /// реализация добавления списка
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AddAll(IEnumerable<T> entity)
        {
            foreach (var ent in entity)
            {
                var entry = Context.Entry(ent);
                entry.State = EntityState.Added;
                _entities.Add(ent);
            }
        }

        /// <summary>
        /// реализация удаления
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEnt(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
        }

        /// <summary>
        /// реализация удаления списка
        /// </summary>
        /// <param name="entity"></param>
        public virtual void DeleteAll(IEnumerable<T> entity)
        {
            foreach (var ent in entity)
            {
                var entry = Context.Entry(ent);
                entry.State = EntityState.Deleted;
                _entities.Remove(ent);
            }
        }

        /// <summary>
        /// реализация редактирования
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Edit(T entity)
        {
            //_context.Entry(entity).State = EntityState.Modified;

            if (entity == null)
                throw new ArgumentNullException("entity");
            Context.Entry(entity).State = EntityState.Modified;
            //this._context.SaveChanges();
        }

        /// <summary>
        /// реализация удаления
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var entity = _entities.Find(id);

            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
        }

        /// <summary>
        /// реализация сохранения
        /// </summary>
        public virtual void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
            }
        }

        /// <summary>
        /// реализация получения единственного значения
        /// </summary>
        /// <param name="where"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        public virtual T GetSingle(Func<T, bool> where,
         params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            IQueryable<T> dbQuery = navigationProperties.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(_entities, (current, navigationProperty) => current.Include<T, object>(navigationProperty));
            item = dbQuery.FirstOrDefault(where);
            return item;
        }
    }
}