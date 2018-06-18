using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.WebUI.Models.Account;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PolyclinicProject.Domain.Common;

namespace PolyclinicProject.WebUI.Controllers
{
    /// <summary>
    /// управление данными для входа и выхода пользователя
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// подключение автомапера
        /// </summary>
        protected AutoMapper.IMapper Mapper;

        /// <summary>
        /// подключение сервиса для управления пользователями
        /// </summary>
        private readonly IUserInfoService _service;

        public AccountController(AutoMapper.IMapper mapper, IUserInfoService service)
        {
            _service = service;
            Mapper = mapper;
        }

        /// <summary>
        /// управление входом
        /// </summary>
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        /// <summary>
        /// управление ролями
        /// </summary>
        private ApplicationRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// управлеие пользователями
        /// </summary>
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        /// <summary>
        /// Вход в систему GET
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl; return View();
        }

        /// <summary>
        /// вход в систему POST
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
                    user.DateLogIn = DateTime.Now;
                    await UserManager.UpdateAsync(user);

                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        var role = await RoleManager.FindByIdAsync(user.RoleInfoId);

                        if (role.Name == "SysAdmin")
                            return RedirectToAction("Index", "SysAdmin");
                        if (role.Name == "Personal")
                            return RedirectToAction("Index", "Personal");
                        if (role.Name == "Provider")
                            return RedirectToAction("Index", "Provider");
                        if (role.Name == "Client")
                            return RedirectToAction("Index", "Client");
                    }

                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}