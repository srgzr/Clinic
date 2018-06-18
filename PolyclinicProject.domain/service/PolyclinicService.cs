using System;
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
    public class PolyclinicService : GenericService<Polyclinic>, IPolyclinicService
    {
        public PolyclinicService(IDbContext context) : base(context)
        {
        }

        /// <summary>
        /// применение подключения ссылок
        /// </summary>
        private readonly Expression<Func<Polyclinic, object>>[] _includeInfo = { b => b.Personals};

        public async Task<PagingOutput<Polyclinic>> GetAllAsync(int page, int pageSize,
            SortOprions sortOrder, string search)
        {
            var query = from _ in Context.Set<Polyclinic>()
                orderby _.Id
                select _;

            if (search != "")
                query = from _ in Context.Set<Polyclinic>()
                        where _.Name.ToLower().Contains(search.ToLower())
                        orderby _.Id
                        select _;

            switch (sortOrder)
            {
                case SortOprions.Name:
                    query = query.OrderBy(x => x.Name);
                    break;

                case SortOprions.Name_desc:
                    query = query.OrderByDescending(x => x.Name);
                    break;

                default:
                    break;
            }

            return await CommonRepositoryAsync.GetAllWithPageAsync(query, page, pageSize, _includeInfo);
        }
    }
}
