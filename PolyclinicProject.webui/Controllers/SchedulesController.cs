using Microsoft.AspNet.Identity;
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
    public class SchedulesController : Controller
    {
        /// <summary>
        /// автомапер
        /// </summary>
        protected AutoMapper.IMapper Mapper;

        /// <summary>
        /// сервис управления
        /// </summary>
        private readonly IScheduleService _scheduleService;
        private readonly IPersonalService _personalService;
   
        public SchedulesController(AutoMapper.IMapper mapper, IScheduleService scheduleService, IPersonalService personalService)
        {
            Mapper = mapper;
            _scheduleService = scheduleService;
            _personalService = personalService;
        }

        /// <summary>
        /// менеджер пользователей
        /// </summary>
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        /// <summary>
        /// Список
        /// </summary>
        /// <param name="page">страница</param>
        /// <param name="pageSize">количество элементов на странице</param>
        /// <param name="sortOrder">парамеры сортировки</param>
        /// <param name="search">парамеры поиска</param>
        /// <returns>Список типов занятий</returns>
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, SortOprions sortOrder = SortOprions.Number, string search = "")
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(search) ? "" : search;
            ViewBag.NameSortParam = sortOrder == SortOprions.Name ? "Name_desc" : "Name";
            ViewBag.CurrentSort = sortOrder;
            var view = await _scheduleService.GetAllAsync(page, pageSize, sortOrder, search);
            return View(view);
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <returns>Список типов занятий</returns>
        [Authorize(Roles = "Personal")]
        public async Task<ActionResult> View()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
                return RedirectToAction("Login", "Account");
            
            var view = await _scheduleService.GetAllForUserAsync(user.Id);
            return View(view);
        }

        /// <summary>
        /// просмотр информации
        /// </summary>
        /// <param name="id">номер объекта</param>
        /// <returns>инфомарция</returns>
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var view = await _scheduleService.GetWithInclude(id ?? 0);
            if (view == null)
            {
                return HttpNotFound();
            }

            return View(view);
        }


        private readonly Expression<Func<Personal, object>>[] _includeUserInfo =
            { b => b.UserInfo, b=>b.UserInfo.RoleInfo, b=>b.Position, b=>b.Polyclinic};

        /// <summary>
        /// создание
        /// </summary>
        /// <returns>пустой объект с заданными начальными параметрами</returns>
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Create()
        {
            var view = new Schedule
            {
                IsActive = true,
            };

            var model = Mapper.Map<Schedule, ScheduleModel>(view);
             model.Personals = _personalService.GetAllWithInclude(_includeUserInfo).ToSelectList();
            return View(model);
        }

        /// <summary>
        /// сохранение занятия
        /// </summary>
        /// <returns>сохранение объекта с заданными параметрами</returns>
        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Create(ScheduleModel model)
        {
            model.Personals = _personalService.GetAll().ToSelectList();
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<ScheduleModel, Schedule>(model);
                if (_scheduleService.GetAll().FirstOrDefault(s => s.PersonalId == model.PersonalId && model.IsFirstShift == s.IsFirstShift && model.Even == s.Even) != null)
                {
                    ModelState.AddModelError(string.Empty, $"Данные для расписания уже были добавлены для врача. Повторное добавление не возможно");
                }

                if (ModelState.IsValid)
                {
                  
                    await _scheduleService.AddAsync(data);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            return View(model);
        }

        /// <summary>
        /// редактирование
        /// </summary>
        /// <returns>объект с заданными начальными параметрами, сохраненные в БД</returns>
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cheque = await _scheduleService.GetAsync(id ?? 0);
            if (cheque == null)
            {
                return HttpNotFound();
            }
            var model = Mapper.Map<Schedule, ScheduleModel>(cheque);
            model.Personals = _personalService.GetAll().ToSelectList();

            return View(model);
        }

        /// <summary>
        ///редактирование
        /// </summary>
        /// <param name="id">код</param>
        /// <param name="model">данные</param>
        /// <returns>измененные объект</returns>
        [Authorize(Roles = "SysAdmin")]
        [HttpPost]
        public async Task<ActionResult> Edit(int id, ScheduleModel model)
        {
            model.Personals = _personalService.GetAll().ToSelectList();

            if (_scheduleService.GetAll().FirstOrDefault(s=>s.PersonalId==model.PersonalId&&model.IsFirstShift==s.IsFirstShift&&model.Even==s.Even&&s.Id!=id)!=null)
            {
                ModelState.AddModelError(string.Empty, $"Данные для расписания уже были добавлены для врача. Повторное добавление не возможно");
            }

            if (ModelState.IsValid)
            {
                var item = Mapper.Map<ScheduleModel, Schedule>(model);
                await _scheduleService.UpdateAsync(item, id);
                return RedirectToAction("Index");
            }
            else return View(model);
        }

        /// <summary>
        /// удаление объкта по id
        /// </summary>
        /// <param name="id">код</param>
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var diplom = await _scheduleService.GetAsync(id ?? 0);
            if (diplom == null)
            {
                return HttpNotFound();
            }
            await _scheduleService.DeleteAsync(diplom);

            return RedirectToAction("Index");
        }
    }
}