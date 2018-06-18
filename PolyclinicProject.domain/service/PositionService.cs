using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Service.Common;

namespace PolyclinicProject.Domain.Service
{
    public class PositionService : GenericService<Position>, IPositionService
    {
       public PositionService(IDbContext context) : base(context)
        {
        }
    }
}