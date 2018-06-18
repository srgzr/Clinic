using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;
using PolyclinicProject.Domain.Service.Common;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Service
{
    /// <summary>
    /// реализация методов для работы с пользователями
    /// </summary>
    public class UserInfoService : GenericService<UserInfo>, IUserInfoService
    {
        public UserInfoService(IDbContext context) : base(context)
        {
        }

        /// <summary>
        /// применение подключения ссылок
        /// </summary>
        private readonly Expression<Func<UserInfo, object>>[] _includeUserInfo = { b => b.RoleInfo };

        /// <summary>
        /// постраничное получение данных
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PagingOutput<UserInfo>> GetAllAsync(int page, int pageSize, SortOprions sort, string search = "")
        {
            var query = from _ in Context.Set<UserInfo>()
                        where _.RoleInfoId == 1
                        orderby _.Id
                        select _;

            if (search != "")
                query = from _ in Context.Set<UserInfo>()
                        where _.SurName.ToLower().Contains(search.ToLower())
                        || _.LastName.ToLower().Contains(search.ToLower())
                        || _.FirstName.ToLower().Contains(search.ToLower())
                        || _.PhoneNumber.ToLower().Contains(search.ToLower())
                        || _.Email.ToLower().Contains(search.ToLower())
                        orderby _.Id
                        select _;

            switch (sort)
            {
                case SortOprions.Email:
                    query = query.OrderBy(x => x.Email);
                    break;

                case SortOprions.Email_desc:
                    query = query.OrderByDescending(x => x.Email);
                    break;

                case SortOprions.Name:
                    query = query.OrderBy(x => x.FirstName);
                    break;

                case SortOprions.Name_desc:
                    query = query.OrderByDescending(x => x.FirstName);
                    break;

                case SortOprions.Number:
                    query = query.OrderBy(x => x.Id);
                    break;

                case SortOprions.Number_desc:
                    query = query.OrderByDescending(x => x.Id);
                    break;

                case SortOprions.PhoneNumber:
                    query = query.OrderBy(x => x.PhoneNumber);
                    break;

                case SortOprions.PhoneNumber_desc:
                    query = query.OrderByDescending(x => x.PhoneNumber);
                    break;

                default:
                    break;
            }

            return await CommonRepositoryAsync.GetAllWithPageAsync(query, page, pageSize, _includeUserInfo);
        }

        /// <summary>
        /// получение данных с вклбченными элементами
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetWithInclude(int id, params Expression<Func<UserInfo, object>>[] includes)
        {
            var query = Context.Set<UserInfo>().AsNoTracking().OrderBy(s => s.Id);
            var data = await CommonRepositoryAsync.GetIncludeAsync(query, _includeUserInfo);
            var result = data.FirstOrDefault(s => s.Id == id);
            Context.Entry(result).State = EntityState.Detached;
            return result;
        }

        public async Task<PagingOutput<UserInfo>> GetAllClientsAsync(int page, int pageSize, SortOprions sort, string search = "")
        {
            var query = from _ in Context.Set<UserInfo>()
                       where _.RoleInfoId == 4
                       orderby _.Id
                       select _;

            if (search != "")
                query = from _ in Context.Set<UserInfo>()
                        where _.SurName.ToLower().Contains(search.ToLower())
                        || _.LastName.ToLower().Contains(search.ToLower())
                        || _.FirstName.ToLower().Contains(search.ToLower())
                        || _.PhoneNumber.ToLower().Contains(search.ToLower())
                        || _.Email.ToLower().Contains(search.ToLower())
                        orderby _.Id
                        select _;

            switch (sort)
            {
                case SortOprions.Email:
                    query = query.OrderBy(x => x.Email);
                    break;

                case SortOprions.Email_desc:
                    query = query.OrderByDescending(x => x.Email);
                    break;

                case SortOprions.Name:
                    query = query.OrderBy(x => x.FirstName);
                    break;

                case SortOprions.Name_desc:
                    query = query.OrderByDescending(x => x.FirstName);
                    break;

                case SortOprions.Number:
                    query = query.OrderBy(x => x.Id);
                    break;

                case SortOprions.Number_desc:
                    query = query.OrderByDescending(x => x.Id);
                    break;

                case SortOprions.PhoneNumber:
                    query = query.OrderBy(x => x.PhoneNumber);
                    break;

                case SortOprions.PhoneNumber_desc:
                    query = query.OrderByDescending(x => x.PhoneNumber);
                    break;

                default:
                    break;
            }

            return await CommonRepositoryAsync.GetAllWithPageAsync(query, page, pageSize, _includeUserInfo);
        }
    }
}