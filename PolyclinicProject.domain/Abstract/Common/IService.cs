using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PolyclinicProject.Domain.Abstract
{
    /// <summary>
    /// набор методов для работы
    /// </summary>
    /// <typeparam name="T">любой класс можно передавать для работы</typeparam>
    public interface IService<T> : IServiceAsync<T> where T : class, new()
    {
        /// <summary>
        /// получение списка данных
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// поиск по Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// поиск
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// добавление
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// редактирование
        /// </summary>
        /// <param name="entity"></param>
        void Edit(T entity);

        /// <summary>
        /// удаление
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// удаление
        /// </summary>
        /// <param name="item"></param>
        void DeleteEnt(T item);

        /// <summary>
        /// сохранение
        /// </summary>
        void Save();

        /// <summary>
        /// получение одного значения
        /// </summary>
        /// <param name="where"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);

        /// <summary>
        /// удаление выбранных значений
        /// </summary>
        /// <param name="entity"></param>
        void DeleteAll(IEnumerable<T> entity);

        /// <summary>
        /// добавление выбранных значений
        /// </summary>
        /// <param name="entity"></param>
        void AddAll(IEnumerable<T> entity);
    }
}