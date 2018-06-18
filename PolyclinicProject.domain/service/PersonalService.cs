using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;
using PolyclinicProject.Domain.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Service
{
    public class PersonalService : GenericService<Personal>, IPersonalService
    {
        public PersonalService(IDbContext context) : base(context)
        {
        }

        /// <summary>
        /// применение подключения ссылок
        /// </summary>
        private readonly Expression<Func<Personal, object>>[] _includeUserInfo = { b => b.UserInfo, b => b.UserInfo.RoleInfo, b => b.Position, b=> b.Polyclinic
        };

        public async Task<Personal> GetWithInclude(int id, params Expression<Func<Personal, object>>[] includes)
        {
            var query = Context.Set<Personal>().OrderBy(s => s.Id);
            var data = await CommonRepositoryAsync.GetIncludeAsync(query, _includeUserInfo);

            return data.FirstOrDefault(s => s.Id == id);
        }

        public override IEnumerable<Personal> GetAll()
        {
            var query = from _ in Context.Set<Personal>().Include("UserInfo").Include("Polyclinic").Include("Position")
                        orderby _.Id
                        select _;

            return query;
        }


        private async Task<PagingOutput<Personal>> GetAllFilterAsync(IOrderedQueryable<Personal> query, int page, int pageSize, SortOprions sort, string search)
        {
            if (search != "")
                query = from _ in Context.Set<Personal>()
                        where _.UserInfo.SurName.ToLower().Contains(search.ToLower())
                        || _.UserInfo.LastName.ToLower().Contains(search.ToLower())
                        || _.UserInfo.FirstName.ToLower().Contains(search.ToLower())
                        || _.UserInfo.PhoneNumber.ToLower().Contains(search.ToLower())
                        || _.UserInfo.Email.ToLower().Contains(search.ToLower())
                        orderby _.Id
                        select _;

            switch (sort)
            {
                case SortOprions.Email:
                    query = query.OrderBy(x => x.UserInfo.Email);
                    break;

                case SortOprions.Email_desc:
                    query = query.OrderByDescending(x => x.UserInfo.Email);
                    break;

                case SortOprions.Name:
                    query = query.OrderBy(x => x.UserInfo.FirstName);
                    break;

                case SortOprions.Name_desc:
                    query = query.OrderByDescending(x => x.UserInfo.FirstName);
                    break;

                case SortOprions.Number:
                    query = query.OrderBy(x => x.Id);
                    break;

                case SortOprions.Number_desc:
                    query = query.OrderByDescending(x => x.Id);
                    break;

                case SortOprions.PhoneNumber:
                    query = query.OrderBy(x => x.UserInfo.PhoneNumber);
                    break;

                case SortOprions.PhoneNumber_desc:
                    query = query.OrderByDescending(x => x.UserInfo.PhoneNumber);
                    break;

                case SortOprions.Polyclinic:
                    query = query.OrderBy(x => x.Polyclinic.Name);
                    break;

                case SortOprions.Polyclinic_desc:
                    query = query.OrderByDescending(x => x.Polyclinic.Name);
                    break;

                default:
                    break;
            }

            return await CommonRepositoryAsync.GetAllWithPageAsync(query, page, pageSize, _includeUserInfo);
        }

        public async Task<PagingOutput<Personal>> GetAllAsync(int page, int pageSize, SortOprions sort, string search)
        {
            var query = from _ in Context.Set<Personal>()
                        orderby _.Id
                        select _;

            return await GetAllFilterAsync(query, page, pageSize, sort, search);
        }

        public async Task<PagingOutput<Personal>> GetAllForPoliclinicAsync(int policlinicId, int page, int pageSize, SortOprions sort, string search)
        {
            var query = from _ in Context.Set<Personal>()
                        where _.PolyclinicId == policlinicId
                        orderby _.Id
                        select _;

            return await GetAllFilterAsync(query, page, pageSize, sort, search);
        }
    }
}