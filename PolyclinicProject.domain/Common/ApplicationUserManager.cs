using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Service.Common;

namespace PolyclinicProject.Domain.Common
{
    public class ApplicationUserManager : UserManager<UserInfo, int>
    {
        public ApplicationUserManager(IUserStore<UserInfo, int> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var db = context.Get<EFDbContext>();
            var manager = new ApplicationUserManager(new UserStore<UserInfo, RoleInfo, int, AppUserLogin, AppUserRole, AppUserClaim>(db));
            return manager;
        }
    }

    public class ApplicationRoleManager : RoleManager<RoleInfo, int>
    {
        public ApplicationRoleManager(RoleStore<RoleInfo, int, AppUserRole> store) : base(store)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<RoleInfo, int, AppUserRole>(context.Get<EFDbContext>()));
        }
    }
}