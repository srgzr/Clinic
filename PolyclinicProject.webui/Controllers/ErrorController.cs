using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Controllers
{
    /// <summary>
    /// контроллер для управлеия ошибками
    /// </summary>
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Error 404 - File not Found";
            return View();
        }

        public ActionResult NotFound404()
        {
            ViewBag.Title = "Error 404 - File not Found";
            return View("Index");
        }
    }
}