using Microsoft.AspNet.Identity.Owin;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.WebUI.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Controllers
{
    [Authorize(Roles = "SysAdmin")]
    public class PositionsController : Controller
    {
        /// <summary>
        /// автомапер
        /// </summary>
        protected AutoMapper.IMapper Mapper;

        /// <summary>
        /// сервис управления
        /// </summary>
        private readonly IPositionService _positionService;

        public PositionsController(AutoMapper.IMapper mapper, IPositionService positionService)
        {
            Mapper = mapper;
            _positionService = positionService;
        }

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
            var data = await _positionService.GetAsync(id);
            if (data != null)
            {
                data.IsActive = data.IsActive != true;
                await _positionService.UpdateAsync(data, data.Id);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <param name="page">страница</param>
        /// <param name="pageSize">количество элементов на странице</param>
        /// <returns>Список типов занятий</returns>
        public async Task<ViewResult> Index(int page = 1, int pageSize = 5)
        {
            var data = await _positionService.GetAllAsync();
            var total = data.Count();

            var model = new PagingOutput<Position>
            {
                Data = data,
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = total
            };

            return View(model);
        }

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
            var view = await _positionService.GetAsync(id ?? 0);
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
        public ActionResult Create()
        {
            var view = new Position
            {
                IsActive = true
            };

            var model = Mapper.Map<Position, PositionModel>(view);

            return View(model);
        }

        /// <summary>
        /// сохранение занятия по фитнесу
        /// </summary>
        /// <returns>сохранение объекта с заданными параметрами</returns>
        [HttpPost]
        public async Task<ActionResult> Create(PositionModel model)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<PositionModel, Position>(model);

                if (_positionService.GetAll().FirstOrDefault(s => s.Name.ToLower() == model.Name?.ToLower()) == null)
                {
                    if (!ModelState.IsValid) return View(model);

                    await _positionService.AddAsync(data);

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Name", "Такая отдел зарегистрирована в системе. Пожалуйста, введите другое наименование!");
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
            var position = await _positionService.GetAsync(id ?? 0);

            if (position == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<Position, PositionModel>(position);

            return View(model);
        }

        /// <summary>
        ///редактирование
        /// </summary>
        /// <param name="id">код</param>
        /// <param name="model">данные</param>
        /// <returns>измененные объект</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(int id, PositionModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                var data = Mapper.Map<PositionModel, Position>(model);
                await _positionService.UpdateAsync(data, id);
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

            var position = await _positionService.GetAsync(id ?? 0);
            if (position == null)
            {
                return HttpNotFound();
            }
            await _positionService.DeleteAsync(position);

            return RedirectToAction("Index");
        }
    }
}