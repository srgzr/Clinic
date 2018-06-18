using Microsoft.AspNet.Identity.Owin;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;
using PolyclinicProject.Domain.Extensions;
using PolyclinicProject.WebUI.Models.User;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PolyclinicProject.Domain.Common;

namespace PolyclinicProject.WebUI.Controllers
{
    /// <summary>
    /// контроллер управления парамерами системных администраторов
    /// </summary>
    [Authorize(Roles = "SysAdmin")]
    public class AdminsController : Controller
    {
        /// <summary>
        /// маппер
        /// </summary>
        protected AutoMapper.IMapper Mapper;

        /// <summary>
        /// сервис управления пользователями
        /// </summary>
        private readonly IUserInfoService _service;

        public AdminsController(AutoMapper.IMapper mapper, IUserInfoService service)
        {
            _service = service;
            Mapper = mapper;
        }

        /// <summary>
        /// менеджер ролей
        /// </summary>
        private ApplicationRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        /// <summary>
        /// менеджер пользователей
        /// </summary>
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        /// <summary>
        /// просмотр информации о пользователях
        /// </summary>
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, SortOprions sortOrder = SortOprions.Number, string search = "")
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(search) ? "" : search;
            ViewBag.NameSortParam = sortOrder == SortOprions.Name ? "Name_desc" : "Name";
            ViewBag.PhoneNumberSortParam = sortOrder == SortOprions.PhoneNumber ? "PhoneNumber_desc" : "PhoneNumber";
            ViewBag.EmailSortParam = sortOrder == SortOprions.Email ? "Email_desc" : "Email";
            ViewBag.CurrentSort = sortOrder;

            var view = await _service.GetAllAsync(page, pageSize, sortOrder, search);

            return View(view);
        }

        private readonly Expression<Func<UserInfo, object>>[] _includeUserInfo = { _ => _.RoleInfo };

        /// <summary>
        /// регистрация
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            var model = new RegisterModel
            {
                Sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => v.ToString()).ToList().ToSelectList(),
            };
            return View(model);
        }

        /// <summary>
        /// регистрация POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _service.FindAsync(s => s.PhoneNumber.ToLower() == model.PhoneNumber.ToLower()) != null)
                {
                    ModelState.AddModelError("PhoneNumber", "Такой пользователь уже существует в системе. Пожалуйста, введите другой номер телефона!");
                }
                else if (await _service.FindAsync(s => s.Email.ToLower() == model.Email.ToLower()) != null)
                {
                    ModelState.AddModelError("Email", "Такой пользователь уже существует в системе. Пожалуйста, введите другой эл. адрес почты!");
                }
                else if (await _service.FindAsync(s => s.UserName.ToLower() == model.UserName.ToLower()) != null)
                {
                    ModelState.AddModelError("UserName", "Такой пользователь уже существует в системе. Пожалуйста, введите другой логин!");
                }
                else
                {
                    var sysAdminRole = RoleManager.Roles.FirstOrDefault(s => s.Name == "SysAdmin");
                    model.DateRegistration = DateTime.Now;
                    model.RoleId = sysAdminRole.Id;
                    var user = Mapper.Map<RegisterModel, UserInfo>(model);
                    user.RoleInfo = sysAdminRole;
                    user.EmailConfirmed = true;
                    user.PhoneNumberConfirmed = true;
                    var result = await UserManager.CreateAsync(user, model.Password);
                    var role = await RoleManager.FindByIdAsync(user.RoleInfoId);
                    await UserManager.AddToRoleAsync(user.Id, role.Name);

                    if (!await UserManager.IsInRoleAsync(user.Id, role.Name))
                    {
                        await UserManager.AddToRoleAsync(user.Id, role.Name);
                    }

                    if (result.Succeeded) { return RedirectToAction("Login", "Account"); }
                    else
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        /// <summary>
        /// изменение состояния
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ActiveState(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = user.IsActive != true;
                await UserManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// удаление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed()
        {
            var user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded) { return RedirectToAction("Logout", "Account"); }
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// изменение
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int id)
        {
            var user = await _service.GetWithInclude(id, _includeUserInfo);
            if (user != null)
            {
                var model = Mapper.Map<UserInfo, EditModel>(user);
                model.Sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => v.ToString()).ToList().ToSelectList();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// изменение
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Detail(int id)
        {
            var user = await _service.GetWithInclude(id, _includeUserInfo);
            if (user != null)
            {
                var model = Mapper.Map<UserInfo, EditModel>(user);
                model.Sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => v.ToString()).ToList().ToSelectList();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// изменение POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(EditModel model)
        {
            var sysAdminRole = RoleManager.Roles.FirstOrDefault(s => s.Name == "SysAdmin");
            if (sysAdminRole != null)
            {
                model.RoleInfoId = sysAdminRole.Id;
                var user = Mapper.Map<EditModel, UserInfo>(model);
                user.RoleInfo = sysAdminRole;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ошибка обновления данных пользователя");
                }
            }

            return View(model);
        }
    }
}