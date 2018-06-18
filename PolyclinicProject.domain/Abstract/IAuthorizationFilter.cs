using System.Web.Mvc;

namespace PolyclinicProject.Domain.Abstract
{
    /// <summary>
    /// авторизация пользователя
    /// </summary>
    public interface IAuthorizationFilter
    {
        /// <summary>
        /// метод авторизации пользователей
        /// </summary>
        /// <param name="filterContext">параметры</param>
        void OnAuthorization(AuthorizationContext filterContext);
    }
}