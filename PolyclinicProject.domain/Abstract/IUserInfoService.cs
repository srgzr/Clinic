using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Abstract
{
    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public interface IUserInfoService : IService<UserInfo>
    {
        Task<PagingOutput<UserInfo>> GetAllAsync(int page, int pageSize, SortOprions sort, string search = "");

        Task<UserInfo> GetWithInclude(int id, params Expression<Func<UserInfo, object>>[] includes);

        Task<PagingOutput<UserInfo>> GetAllClientsAsync(int page, int pageSize, SortOprions sort, string search = "");
    }
}