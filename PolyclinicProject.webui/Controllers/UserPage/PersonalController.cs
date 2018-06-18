using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Controllers
{
   // [Authorize(Roles = "Personal")]
    public class PersonalController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}