using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;
using PolyclinicProject.Domain.Service.Common;

namespace PolyclinicProject.Domain.Service
{
   public class ScheduleService : GenericService<Schedule>, IScheduleService
    {
        public ScheduleService(IDbContext context) : base(context)
        {
        }

        /// <summary>
        /// применение подключения ссылок
        /// </summary>
        private readonly Expression<Func<Schedule, object>>[] _include =
        {
            b => b.Personal, b => b.Personal.UserInfo, b => b.Personal.UserInfo.RoleInfo,
            b => b.Personal.Polyclinic, b => b.Personal.Position
        };

        private async Task<PagingOutput<Schedule>> GetAllFilterAsync(IOrderedQueryable<Schedule> query,  int page, int pageSize,
            SortOprions sort, string search = "")
        {

            if (search != "")
                query = from _ in Context.Set<Schedule>()
                        where _.Personal.UserInfo.SurName.ToLower().Contains(search.ToLower())
                        || _.Personal.UserInfo.LastName.ToLower().Contains(search.ToLower())
                        || _.Personal.UserInfo.FirstName.ToLower().Contains(search.ToLower())
                        || _.Personal.UserInfo.PhoneNumber.ToLower().Contains(search.ToLower())
                        || _.Personal.UserInfo.Email.ToLower().Contains(search.ToLower())
                              || _.Personal.Polyclinic.Name.ToLower().Contains(search.ToLower())
                              || _.Personal.Position.Name.ToLower().Contains(search.ToLower())
                              || _.Personal.Polyclinic.Address.ToLower().Contains(search.ToLower())
                        orderby _.Id
                        select _;

            switch (sort)
            {
                case SortOprions.Email:
                    query = query.OrderBy(x => x.Personal.UserInfo.Email);
                    break;

                case SortOprions.Email_desc:
                    query = query.OrderByDescending(x => x.Personal.UserInfo.Email);
                    break;

                case SortOprions.Name:
                    query = query.OrderBy(x => x.Personal.UserInfo.FirstName);
                    break;

                case SortOprions.Name_desc:
                    query = query.OrderByDescending(x => x.Personal.UserInfo.FirstName);
                    break;

                case SortOprions.Number:
                    query = query.OrderBy(x => x.Id);
                    break;

                case SortOprions.Number_desc:
                    query = query.OrderByDescending(x => x.Id);
                    break;

                case SortOprions.Position:
                    query = query.OrderBy(x => x.Personal.Position.Name);
                    break;

                case SortOprions.Position_desc:
                    query = query.OrderByDescending(x => x.Personal.Position.Name);
                    break;

                default:
                    break;
            }

            return await CommonRepositoryAsync.GetAllWithPageAsync(query, page, pageSize, _include);
        }

        public async Task<PagingOutput<Schedule>> GetAllForPoloclinicAsync(int policlinicId, int page, int pageSize, SortOprions sort, string search = "")
        {
            IOrderedQueryable<Schedule> query = from _ in Context.Set<Schedule>()
                        where _.Personal.PolyclinicId == policlinicId
                        orderby _.Id
                        select _;

          return  await GetAllFilterAsync(query, page, pageSize, sort, search);
        }

        public async Task<PagingOutput<Schedule>> GetAllAsync(int page, int pageSize, SortOprions sort, string search = "")
        {
            var query = from _ in Context.Set<Schedule>()
                        orderby _.Id
                        select _;

            return await GetAllFilterAsync(query, page, pageSize, sort, search);
        }

        public async Task<PagingOutput<Schedule>> GetAllForUserAsync(int userId)
        {
            var query = from _ in Context.Set<Schedule>()
                        where _.Personal.UserInfoId == userId
                        orderby _.Id
                        select _;

            return await CommonRepositoryAsync.GetAllWithPageAsync(query, 1, 10, _include);
        }

        public async Task<Schedule> GetWithInclude(int id, params Expression<Func<Schedule, object>>[] includes)
        {
            var query = Context.Set<Schedule>().AsNoTracking().OrderBy(s => s.Id);
            var data = await CommonRepositoryAsync.GetIncludeAsync(query, _include);
            var result = data.FirstOrDefault(s => s.Id == id);
            Context.Entry(result).State = EntityState.Detached;
            return result;
        }

        public async Task<PagingOutput<Schedule>> GetAllForPersonalAsync(int id)
        {
            var query = from _ in Context.Set<Schedule>()
                        where _.PersonalId == id
                        orderby _.Id
                        select _;

            return await CommonRepositoryAsync.GetAllWithPageAsync(query, 1, 10, _include);
        }
    }
}
