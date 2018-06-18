using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Controllers
{
   // [Authorize(Roles = "SysAdmin")]
    public class SysAdminController : Controller
    {
        // GET: SysAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}