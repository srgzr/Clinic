using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.WebUI.Models;

namespace PolyclinicProject.WebUI.Controllers
{
    /// <summary>
    /// Контроллер управления магазинами
    /// </summary>
    [Authorize(Roles = "SysAdmin")]
    public class PolyclinicsController : Controller
    {
        /// <summary>
        /// автомапер
        /// </summary>
        protected AutoMapper.IMapper Mapper;

        /// <summary>
        /// сервис управления
        /// </summary>
        private readonly IPolyclinicService _polyclinicService;

        public PolyclinicsController(AutoMapper.IMapper mapper, IPolyclinicService polyclinicService)
        {
            Mapper = mapper;
            _polyclinicService = polyclinicService;
        }

        /// <summary>
        /// изменение состояния
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ActiveState(int id)
        {
            var data = await _polyclinicService.GetAsync(id);
            if (data != null)
            {
                data.IsActive = data.IsActive != true;
                await _polyclinicService.UpdateAsync(data, data.Id);
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
            var data = await _polyclinicService.GetAllAsync();
            var total = data.Count();

            var model = new PagingOutput<Polyclinic>
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
            var view = await _polyclinicService.GetAsync(id ?? 0);
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
            var view = new Polyclinic
            {
                IsActive = true
            };

            var model = Mapper.Map<Polyclinic, PolyclinicModel>(view);

            return View(model);
        }

        /// <summary>
        /// сохранение занятия по фитнесу
        /// </summary>
        /// <returns>сохранение объекта с заданными параметрами</returns>
        [HttpPost]
        public async Task<ActionResult> Create(PolyclinicModel model)
        {
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<PolyclinicModel, Polyclinic>(model);

                if (_polyclinicService.GetAll().FirstOrDefault(s => s.Name.ToLower() == model.Name?.ToLower()) == null)
                {
                    if (!ModelState.IsValid) return View(model);

                    await _polyclinicService.AddAsync(data);

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Name", "Такой магазин уже зарегистрирован. Пожалуйста, введите другое наименование!");
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
            var shop = await _polyclinicService.GetAsync(id ?? 0);

            if (shop == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<Polyclinic, PolyclinicModel>(shop);

            return View(model);
        }

        /// <summary>
        ///редактирование
        /// </summary>
        /// <param name="id">код</param>
        /// <param name="model">данные</param>
        /// <returns>измененные объект</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(int id, PolyclinicModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                var data = Mapper.Map<PolyclinicModel, Polyclinic>(model);
                await _polyclinicService.UpdateAsync(data, id);
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

            var Shop = await _polyclinicService.GetAsync(id ?? 0);
            if (Shop == null)
            {
                return HttpNotFound();
            }
            await _polyclinicService.DeleteAsync(Shop);

            return RedirectToAction("Index");
        }
    }
}