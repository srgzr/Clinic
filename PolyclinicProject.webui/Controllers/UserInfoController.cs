using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.WebUI.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PolyclinicProject.Domain.Common;

namespace PolyclinicProject.WebUI.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями
    /// </summary>
    [Authorize]
    public class UserInfoController : Controller
    {
        /// <summary>
        /// подключение сервиса для работы с пользователями
        /// </summary>
        private readonly IUserInfoService _userService;

        public UserInfoController(IUserInfoService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// менеджер пользователей
        /// </summary>
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public async Task<ActionResult> EditPersonalInfo()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
                return RedirectToAction("Login", "Account");

            int? id = user.Id;

            var view = await _userService.GetAsync(id ?? 0);
            if (view == null)
            {
                return HttpNotFound();
            }
            var userInfo = new ModelUser
            {
                Birthday = view.Birthday,
                DateLogIn = user.DateLogIn,
                DateRegistration = user.DateRegistration,
                Email = view.Email,
                FirstName = view.FirstName,
                Id = view.Id,
                IsActiv = user.IsActive,
                LastName = view.LastName,
                UserName = user.UserName,
                Password = view.Password,
                PasswordConfirm = view.Password,
                RoleId = user.RoleInfoId,
                SurName = view.SurName,
                PhoneNumber = view.PhoneNumber,
                Address = view.Address
            };

            return View(userInfo);
        }

        /// <summary>
        /// изменение POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditPersonalInfo(ModelUser model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            user.Password = model.Password;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.Birthday = model.Birthday;
            user.Address = model.Address;

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("EditPersonalInfo", "UserInfo");
            }

            ModelState.AddModelError(string.Empty, "Ошибка обновления данных пользователя");

            return View(model);
        }
    }
}