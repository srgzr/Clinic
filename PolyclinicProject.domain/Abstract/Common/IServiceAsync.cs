using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Abstract
{
    public interface IServiceAsync<T> where T : class, new()
    {
        /// <summary>
        /// асинхронное получение списка данных
        /// </summary>
        /// <returns></returns>
        Task<ICollection<T>> GetAllAsync();

        /// <summary>
        /// асинхронный поиск по Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<T> GetAsync(int id);

        /// <summary>
        /// асинхронный поиск 1 значения
        /// </summary>
        /// <param name="match">параметры</param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> match);

        /// <summary>
        /// асинхронный поиск всех значений
        /// </summary>
        /// <param name="match">параметры</param>
        /// <returns></returns>
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        /// <summary>
        /// асинхронное добавление
        /// </summary>
        /// <param name="entity"></param>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// асинхронное добавление выбранных значений
        /// </summary>
        /// <param name="entity"></param>
        Task AddAllAsync(IEnumerable<T> entity);

        /// <summary>
        /// асинхронное редактирование
        /// </summary>
        /// <param name="updated"></param>
        /// <param name="key"></param>
        Task<T> UpdateAsync(T updated, int key);

        /// <summary>
        /// асинхронное удаление
        /// </summary>
        /// <param name="t"></param>
        Task<int> DeleteAsync(T t);

        /// <summary>
        /// асинхронное удаление
        /// </summary>
        /// <param>
        ///     <name>item</name>
        /// </param>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        /// <summary>
        /// асинхронное сохранение
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// асинхронное постраничное получение данных
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagingOutput<T>> GetAllAsync(int page, int pageSize);

        /// <summary>
        /// асинхронное получение данных с включением
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<T> GetAllWithInclude(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// получение значение по ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<T> GetWithIncludeAsync<T>(int id, params Expression<Func<T, object>>[] includes) where T : CommanEntity;
    }
}