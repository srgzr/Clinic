using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;

namespace PolyclinicProject.Domain.Abstract
{
  public  interface IScheduleService : IService<Schedule>
    {
        Task<PagingOutput<Schedule>> GetAllAsync(int page, int pageSize, SortOprions sort, string search = "");
        Task<PagingOutput<Schedule>> GetAllForPoloclinicAsync(int policlinicId, int page, int pageSize, SortOprions sort, string search = "");
        Task<PagingOutput<Schedule>> GetAllForUserAsync(int userId);
        Task<PagingOutput<Schedule>> GetAllForPersonalAsync(int id);
        Task<Schedule> GetWithInclude(int id, params Expression<Func<Schedule, object>>[] includes);
    }
}
