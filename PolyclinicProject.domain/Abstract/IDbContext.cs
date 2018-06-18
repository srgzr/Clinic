using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Abstract
{
    /// <summary>
    /// контекст для работы с БД
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// создание связи классов и бд
        /// </summary>
        /// <typeparam name="T">класс(сущность)</typeparam>
        /// <returns></returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// создание параметров для контекста
        /// </summary>
        /// <typeparam name="T">класс(сущность)</typeparam>
        /// <param name="entity">класс(сущность)</param>
        /// <returns></returns>
        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        /// <summary>
        /// сохранение изменений в бд
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Асинхронное сохранение
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// для очистки неуправляемых ресурсов, которые используются в приложении
        /// </summary>
        void Dispose();
    }
}