using Microsoft.AspNet.Identity.Owin;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Common;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.WebUI.Models.Role;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Controllers
{
    /// <summary>
    /// контроллер управления ролями пользователей системы
    /// </summary>
    [Authorize(Roles = "SysAdmin")]
    public class RolesController : Controller
    {
        /// <summary>
        /// автомапер
        /// </summary>
        protected AutoMapper.IMapper Mapper;

        /// <summary>
        /// подключение сервиса для работы с ролями
        /// </summary>
        private readonly IRoleInfoService _roleInfoService;

        public RolesController(AutoMapper.IMapper mapper, IRoleInfoService service)
        {
            Mapper = mapper;
            _roleInfoService = service;
        }

        /// <summary>
        /// формирование менеджера ролей
        /// </summary>
        private ApplicationRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        /// <summary>
        /// просмотр информации о ролях
        /// </summary>
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var data = RoleManager.Roles;
            var total = data.Count();

            var model = new PagingOutput<RoleInfo>
            {
                Data = data,
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = total
            };

            return View(model);
        }

        /// <summary>
        /// создание GET
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var model = new CreateRoleModel
            {
                IsActive = true
            };
            return View(model);
        }

        /// <summary>
        /// создание POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                if (_roleInfoService.GetSingle(s => s.Name.ToLower() == model?.Name?.ToLower()) == null)
                {
                    var role = Mapper.Map<CreateRoleModel, RoleInfo>(model);
                    var result = await RoleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Произошла ошибка");
                    }
                }
                ModelState.AddModelError("Name", "Такая роль пользователя уже существует в системе. Пожалуйста, введите другое наименование роли");
            }
            return View(model);
        }

        /// <summary>
        /// редактирование данных GET
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var role = _roleInfoService.GetById(id);
            if (role != null)
            {
                var roleModel = Mapper.Map<RoleInfo, EditRoleModel>(role);
                return View(roleModel);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// редактирование данных
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(EditRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var role = Mapper.Map<EditRoleModel, RoleInfo>(model);
                await _roleInfoService.UpdateAsync(role, role.Id);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        /// <summary>
        /// изменение состояния
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ActiveState(int id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                role.IsActive = role.IsActive != true;
                await RoleManager.UpdateAsync(role);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// удаление данных
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await RoleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}