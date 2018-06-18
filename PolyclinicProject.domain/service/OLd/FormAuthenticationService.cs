//using System;
//using System.Linq;
//using System.Web;
//using System.Web.Security;
//using FitnesApp.Domain.Abstract;
//using FitnesApp.Domain.Entities;

//namespace FitnesApp.Domain.Service
//{
//    public class FormAuthenticationService : IAuthenticationProvider
//    {
//        private const string AuthCookieName = "AuthCookie";
//        private UserInfo _currentUser;
//        private readonly IUserInfoService _userService;

//        /// <summary>
//        /// конструктор
//        /// </summary>
//        /// <param name="user"></param>
//        public FormAuthenticationService(IUserInfoService user)
//        {
//            _userService = user;
//        }

//        /// <summary>
//        /// реализация аутентификации пользователя
//        /// </summary>
//        /// <param name="username">имя</param>
//        /// <param name="password">пароль</param>
//        /// <param name="rememberMe">запомнить меня</param>
//        /// <returns></returns>
//        public bool Authenticate(string username, string password, bool rememberMe)
//        {
//            var result = false;
//            var usr = _userService.GetAll().FirstOrDefault(s => s.Password == password && s.Login == username);
//            if (usr != null)
//            {
//                result = true;
//                Login(usr, rememberMe);
//            }

//            if (result)
//                FormsAuthentication.SetAuthCookie(username, false);
//            return result;
//        }

//        /// <summary>
//        /// реалзация входа в систему
//        /// </summary>
//        /// <param name="user"></param>
//        /// <param name="rememberMe"></param>
//        public void Login(UserInfo user, bool rememberMe)
//        {
//            DateTime expiresDate = DateTime.Now.AddMinutes(30);
//            if (rememberMe)
//                expiresDate = expiresDate.AddDays(10);

//            user.DateLogIn = DateTime.Now;
//            _userService.Edit(user);
//            _userService.Save();

//            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
//          1, // Ticket version
//          user.Id.ToString(), // Username associated with ticket
//          DateTime.Now, // Date/time issued
//          DateTime.Now.AddMinutes(30), // Date/time to expire
//          true, // "true" for a persistent user cookie
//          user?.RoleInfo?.Name, // User-data, in this case the roles
//          FormsAuthentication.FormsCookiePath);// Path cookie valid for


//            // ticket = FormsAuthentication.Decrypt(ticket.Name);
//            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

//            SetValue(AuthCookieName, encryptedTicket, expiresDate);
//            SetValue("id", CurrentUser.Id.ToString(), expiresDate);
//            SetValue("userName", CurrentUser.Login, expiresDate);
//            if (_currentUser.RoleInfo != null && _currentUser != null && _currentUser.RoleInfo.Id == 1)
//                SetValue("Role", "SysAdmin", expiresDate);
//            if (_currentUser.RoleInfo != null && _currentUser != null && _currentUser.RoleInfo.Id == 2)
//                SetValue("Role", "Admin", expiresDate);
//            if (_currentUser.RoleInfo != null && _currentUser != null && _currentUser.RoleInfo.Id == 3)
//                SetValue("Role", "Trener", expiresDate);
//            if (_currentUser.RoleInfo != null && _currentUser != null && _currentUser.RoleInfo.Id == 4)
//                SetValue("Role", "Client", expiresDate);


//            _currentUser = user;
//        }

//        /// <summary>
//        /// реализация выхода из системы
//        /// </summary>
//        public void Logoff()
//        {
//            SetValue(AuthCookieName, null, DateTime.Now.AddYears(-1));
//            SetValue("id", null, DateTime.Now.AddYears(-1));
//            SetValue("userName", null, DateTime.Now.AddYears(-1));
//            SetValue("role", null, DateTime.Now.AddYears(-1));
//            _currentUser = null;
//        }

//        /// <summary>
//        /// генерация пароля
//        /// </summary>
//        /// <param name="pass">Original password</param>
//        /// <param name="salt">User ID + " " + User.ID</param>
//        /// <returns></returns>
//        public string GeneratePassword(string pass, string salt)
//        {
//            return "";//OxoCrypt.MD5(pass + OxoCrypt.MD5(pass + salt + " " + salt));
//        }

//        /// <summary>
//        /// реализация поиска текущего пользователя
//        /// </summary>
//        public UserInfo CurrentUser
//        {
//            get
//            {
//                if (_currentUser != null) return _currentUser;
//                try
//                {
//                    var cookie = HttpContext.Current.Request.Cookies[AuthCookieName] != null ? HttpContext.Current.Request.Cookies[AuthCookieName].Value : null;
//                    if (!string.IsNullOrEmpty(cookie))
//                    {
//                        var ticket = FormsAuthentication.Decrypt(cookie);
//                        _currentUser = _userService.GetSingle(s => ticket != null && s.Id == int.Parse(ticket.Name));
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _currentUser = null;
//                }
//                return _currentUser;
//            }
//        }

//        /// <summary>
//        /// работа с куки
//        /// </summary>
//        /// <param name="cookieName"></param>
//        /// <param name="cookieObject"></param>
//        /// <param name="dateStoreTo"></param>
//        public static void SetValue(string cookieName, string cookieObject, DateTime dateStoreTo)
//        {
//            var cookie = HttpContext.Current.Response.Cookies[cookieName] ?? new HttpCookie(cookieName) { Path = "/" };

//            cookie.Value = cookieObject;
//            cookie.Expires = dateStoreTo;

//            HttpContext.Current.Response.SetCookie(cookie);
//        }
//    }
//}