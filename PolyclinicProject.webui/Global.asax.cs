using System;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PolyclinicProject.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //  private IAuthenticationProvider _authenticationProvider;
        //public MvcApplication(IAuthenticationProvider authenticationProvider)
        //{
        //    _authenticationProvider = authenticationProvider;
        //}

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CultureInfo cultureInfo = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //// Do whatever you want to do with the error

            ////Show the custom error page...
            //Server.ClearError();
            //var routeData = new RouteData();
            //routeData.Values["controller"] = "Error";

            //if (
            //  //  (Context.Server.GetLastError() is HttpException)&&
            //     ((Context.Server.GetLastError() as HttpException)?.GetHttpCode() != 404))
            //{
            //    routeData.Values["action"] = "Index";
            //}
            //else
            //{
            //    // Handle 404 error and response code
            //    Response.StatusCode = 404;
            //    routeData.Values["action"] = "NotFound404";
            //}
            //Response.TrySkipIisCustomErrors = true; // If you are using IIS7, have this line
            //IController errorsController = new ErrorController();
            //HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            //var rc = new RequestContext(wrapper, routeData);
            //errorsController.Execute(rc);
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // ... omitted some code to check user is authenticated

            //   Assert(HttpContext.User.IsInRole("admin"));
        }
    }
}