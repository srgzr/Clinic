using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Entities;
using System;
using System.Web.Security;

namespace PolyclinicProject.WebUI.Provider
{
    public class AppRoleProvider : RoleProvider
    {
        private readonly IRoleInfoService _service;
        private readonly IUserInfoService _userService;

        private AppRoleProvider(IRoleInfoService service, IUserInfoService userInfoService)
        {
            _service = service;
            _userService = userInfoService;
        }

        public override string[] GetRolesForUser(string login)
        {
            string[] role = new string[] { };

            try
            {
                // Получаем пользователя
                UserInfo user = _userService.GetSingle(s => s.UserName == login);
                if (user != null)
                {
                    // получаем роль
                    RoleInfo userRole = _service.GetSingle(s => s.Id == user.RoleInfoId);

                    if (userRole != null)
                    {
                        role = new string[] { userRole.Name };
                    }
                }
            }
            catch
            {
                role = new string[] { };
            }

            return role;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя

            try
            {
                // Получаем пользователя
                UserInfo user = _userService.GetSingle(s => s.UserName == username);
                if (user != null)
                {
                    // получаем роль
                    RoleInfo userRole = _service.GetSingle(s => s.Id == user.RoleInfoId);

                    //сравниваем
                    if (userRole != null && userRole.Name == roleName)
                    {
                        outputResult = true;
                    }
                }
            }
            catch
            {
                outputResult = false;
            }

            return outputResult;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}