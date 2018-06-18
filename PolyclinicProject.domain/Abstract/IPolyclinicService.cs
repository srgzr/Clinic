using System.Threading.Tasks;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;

namespace PolyclinicProject.Domain.Abstract
{
   public interface IPolyclinicService : IService<Polyclinic>
    {
        Task<PagingOutput<Polyclinic>> GetAllAsync(int page, int pageSize, SortOprions sortOrder, string search);
    }
}
