using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Enum;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Controllers
{
    /// <summary>
    /// контроллер для страницы 
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IPolyclinicService _polyclinicService;
        private readonly IPersonalService _personalService;
        private readonly IScheduleService _scheduleService;

        public HomeController(IPolyclinicService polyclinicService, IPersonalService personalService, IScheduleService scheduleService)
        {
            _polyclinicService = polyclinicService;
            _personalService = personalService;
            _scheduleService = scheduleService;
        }

        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, SortOprions sortOrder = SortOprions.Number, string search = "")
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(search) ? "" : search;
            ViewBag.NameSortParam = sortOrder == SortOprions.Name ? "Name_desc" : "Name";
            ViewBag.AddressSortParam = sortOrder == SortOprions.Address ? "Address_desc" : "Address";
            ViewBag.CurrentSort = sortOrder;
            var view = await _polyclinicService.GetAllAsync(page, pageSize, sortOrder, search);
            return View(view);
        }
      
        public async Task<ActionResult> View(int policlinicId, int page = 1, int pageSize = 10, SortOprions sortOrder = SortOprions.Number, string search = "")
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(search) ? "" : search;
            ViewBag.NameSortParam = sortOrder == SortOprions.Name ? "Name_desc" : "Name";
            ViewBag.PhoneNumberSortParam = sortOrder == SortOprions.PhoneNumber ? "PhoneNumber_desc" : "PhoneNumber";
            ViewBag.EmailSortParam = sortOrder == SortOprions.Email ? "Email_desc" : "Email";
            ViewBag.PolyclinicSortParam = sortOrder == SortOprions.Polyclinic ? "Polyclinic_desc" : "Polyclinic";
            ViewBag.PositionSortParam = sortOrder == SortOprions.Position ? "Position_desc" : "Position";
            ViewBag.CurrentSort = sortOrder;
            var view = await _personalService.GetAllForPoliclinicAsync(policlinicId, page, pageSize, sortOrder, search);
            return View(view);
        }

        

        public async Task<ActionResult> Schedules(int id)
        {
            var view = await _scheduleService.GetAllForPersonalAsync(id);
            return View(view);
        }

        public async Task<ActionResult> GetDoctorSchedule(int doctorId)
        {
            var view = await _scheduleService.GetAllForUserAsync(doctorId);
            return View(view);
        }
    }
}