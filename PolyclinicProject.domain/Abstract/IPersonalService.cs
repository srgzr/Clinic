using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolyclinicProject.Domain.Abstract
{
    public interface IPersonalService : IService<Personal>
    {
        Task<Personal> GetWithInclude(int id, params Expression<Func<Personal, object>>[] includes);

        Task<PagingOutput<Personal>> GetAllAsync(int page, int pageSize, SortOprions sortOrder, string search);

        Task<PagingOutput<Personal>> GetAllForPoliclinicAsync(int policlinicId, int page, int pageSize, SortOprions sortOrder, string search);
    }
}