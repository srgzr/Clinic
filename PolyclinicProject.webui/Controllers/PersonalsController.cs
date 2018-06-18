using Microsoft.AspNet.Identity.Owin;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.Domain.Enum;
using PolyclinicProject.Domain.Extensions;
using PolyclinicProject.WebUI.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PolyclinicProject.Domain.Common;

namespace PolyclinicProject.WebUI.Controllers
{
   [Authorize(Roles = "SysAdmin")]
    public class PersonalsController : Controller
    {
        /// <summary>
        /// автомапер
        /// </summary>
        protected AutoMapper.IMapper Mapper;

        /// <summary>
        /// сервис управления
        /// </summary>
        private readonly IPersonalService _personaltService;
        private readonly IPolyclinicService _polyclinicService;
        private readonly IPositionService _positionService;

        public PersonalsController(AutoMapper.IMapper mapper, IPersonalService personalService, 
            IPolyclinicService polyclinicService, IPositionService departmentService)
        {
            Mapper = mapper;
            _personaltService = personalService;
            _polyclinicService = polyclinicService;
            _positionService = departmentService;
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
        /// изменение состояния
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ActiveState(int id)
        {
            var data = await _personaltService.GetAsync(id);
            if (data != null)
            {
                data.IsActive = data.IsActive != true;
                await _personaltService.UpdateAsync(data, data.Id);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <param name="page">страница</param>
        /// <param name="pageSize">количество элементов на странице</param>
        /// <param name="sortOrder"></param>
        /// <returns>Список типов занятий</returns>
        public async Task<ViewResult> Index(int page = 1, int pageSize = 10, SortOprions sortOrder = SortOprions.Number, string search = "")
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(search) ? "" : search;
            ViewBag.NameSortParam = sortOrder == SortOprions.Name ? "Name_desc" : "Name";
            ViewBag.PhoneNumberSortParam = sortOrder == SortOprions.PhoneNumber ? "PhoneNumber_desc" : "PhoneNumber";
            ViewBag.EmailSortParam = sortOrder == SortOprions.Email ? "Email_desc" : "Email";
            ViewBag.PolyclinicSortParam = sortOrder == SortOprions.Polyclinic ? "Polyclinic_desc" : "Polyclinic";
            ViewBag.DepartmentSortParam = sortOrder == SortOprions.Position ? "Position_desc" : "Position";

            ViewBag.CurrentSort = sortOrder;
            var view = await _personaltService.GetAllAsync(page, pageSize, sortOrder, search);
            return View(view);
        }

        private readonly Expression<Func<Personal, object>>[] _includeUserInfo =
        { b => b.UserInfo, b=>b.UserInfo.RoleInfo, b=>b.Position, b=>b.Polyclinic};

        /// <summary>
        /// просмотр информации
        /// </summary>
        /// <param name="id">номер объекта</param>
        /// <returns>инфомарция</returns>
        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var view = await _personaltService.GetWithInclude(id ?? 0, _includeUserInfo);
            if (view == null)
            {
                return HttpNotFound();
            }

            return View(view);
        }

        /// <summary>
        /// создание
        /// </summary>
        /// <returns>пустой объект с заданными начальными параметрами</returns>
        public async Task<ActionResult> Create()
        {
            var view = new Personal
            {
                IsActive = true
            };
            var model = Mapper.Map<Personal, PersonalModel>(view);
            model.Sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => v.ToString()).ToList().ToSelectList();
            model.Polyclinics = (await _polyclinicService.GetAllAsync()).ToSelectList();
            model.Positions = (await _positionService.GetAllAsync()).ToSelectList();
            return View(model);
        }

        /// <summary>
        /// сохранение занятия по фитнесу
        /// </summary>
        /// <returns>сохранение объекта с заданными параметрами</returns>
        [HttpPost]
        public async Task<ActionResult> Create(PersonalModel model)
        {
            model.Sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => v.ToString()).ToList().ToSelectList();
            model.Polyclinics = (await _polyclinicService.GetAllAsync()).ToSelectList();
            model.Positions = (await _positionService.GetAllAsync()).ToSelectList();

            if (ModelState.IsValid)
            {
                if (await _personaltService.FindAsync(s => s.UserInfo.FirstName.ToLower() == model.FirstName.ToLower() &&
                                                           s.UserInfo.LastName.ToLower() == model.LastName.ToLower() &&
                                                           s.UserInfo.SurName.ToLower() == model.SurName.ToLower()
                                                           && s.UserInfo.Birthday == model.Birthday && s.PolyclinicId == model.PolyclinicId) != null)
                {
                    ModelState.AddModelError(string.Empty, "Такой пользователь уже существует в системе. Пожалуйста, введите другие параметры!");
                }
                if (await _personaltService.FindAsync(s => s.UserInfo.PhoneNumber.ToLower() == model.PhoneNumber.ToLower()) != null)
                {
                    ModelState.AddModelError("PhoneNumber", "Такой пользователь уже существует в системе. Пожалуйста, введите другой номер телефона!");
                }
                else if (await _personaltService.FindAsync(s => s.UserInfo.Email.ToLower() == model.Email.ToLower()) != null)
                {
                    ModelState.AddModelError("Email", "Такой пользователь уже существует в системе. Пожалуйста, введите другой эл. адрес почты!");
                }
                else if (await _personaltService.FindAsync(s => s.UserInfo.UserName.ToLower() == model.UserName.ToLower()) != null)
                {
                    ModelState.AddModelError("UserName", "Такой пользователь уже существует в системе. Пожалуйста, введите другой логин!");
                }
                else
                {
                    var personalRole = RoleManager.Roles.FirstOrDefault(s => s.Name == "Personal");
                    model.DateRegistration = DateTime.Now;
                    if (personalRole != null)
                    {
                        model.RoleId = personalRole.Id;
                        var user = Mapper.Map<PersonalModel, UserInfo>(model);
                        user.RoleInfo = personalRole;
                        user.EmailConfirmed = true;
                        user.PhoneNumberConfirmed = true;
                        var result = await UserManager.CreateAsync(user, model.Password);
                        var role = await RoleManager.FindByIdAsync(user.RoleInfoId);
                        await UserManager.AddToRoleAsync(user.Id, role.Name);
                        if (!await UserManager.IsInRoleAsync(user.Id, role.Name))
                        {
                            await UserManager.AddToRoleAsync(user.Id, role.Name);
                        }

                        var personal = Mapper.Map<PersonalModel, Personal>(model);
                        personal.UserInfoId = user.Id;
                        await _personaltService.AddAsync(personal);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Personals");
                        }
                        else
                        {
                            foreach (string error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error);
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        /// <summary>
        /// редактирование
        /// </summary>
        /// <returns>объект с заданными начальными параметрами, сохраненные в БД</returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var personalt = await _personaltService.GetWithInclude(id ?? 0, _includeUserInfo);

            if (personalt == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<Personal, PersonalModel>(personalt);
            model.Sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => v.ToString()).ToList().ToSelectList();
            model.Polyclinics = (await _polyclinicService.GetAllAsync()).ToSelectList();
            model.Positions = (await _positionService.GetAllAsync()).ToSelectList();

            return View(model);
        }

        /// <summary>
        ///редактирование
        /// </summary>
        /// <param name="id">код</param>
        /// <param name="model">данные</param>
        /// <returns>измененные объект</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(int id, PersonalModel model)
        {
            model.Sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => v.ToString()).ToList().ToSelectList();
            model.Polyclinics = (await _polyclinicService.GetAllAsync()).ToSelectList();
            model.Positions = (await _positionService.GetAllAsync()).ToSelectList();

            if (ModelState.IsValid && model != null)
            {
                var user = await UserManager.FindByIdAsync(model.UserInfoId);
                user.PhoneNumber = model.PhoneNumber;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.SurName = model.SurName;
                user.Email = model.Email;
                user.Password = model.Password;
                user.UserName = model.UserName;
                user.IsActive = model.IsActive;
                user.Sex = model.Sex;
                user.Birthday = model.Birthday;
                await UserManager.UpdateAsync(user);
                var data = Mapper.Map<PersonalModel, Personal>(model);
                await _personaltService.UpdateAsync(data, id);
                return RedirectToAction("Index");
            }

            return View();
        }

        /// <summary>
        /// удаление объкта по id
        /// </summary>
        /// <param name="id">код</param>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var personalt = await _personaltService.GetAsync(id ?? 0);
            if (personalt == null)
            {
                return HttpNotFound();
            }
            await _personaltService.DeleteAsync(personalt);

            return RedirectToAction("Index");
        }
    }
}