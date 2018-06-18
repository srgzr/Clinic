using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Service.Common;
using System.Linq;

namespace PolyclinicProject.Domain.Service
{
    /// <summary>
    /// реализация методов для работы с ролями
    /// </summary>
    public class RoleInfoService : GenericService<RoleInfo>, IRoleInfoService
    {
        public RoleInfoService(IDbContext context) : base(context)
        {
        }
    }
}