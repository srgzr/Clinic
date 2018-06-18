using PolyclinicProject.Domain.Entities;

namespace PolyclinicProject.Domain.Abstract
{
    /// <summary>
    /// Методы для работы с пользователями
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// автентификация
        /// </summary>
        /// <param name="username">имя</param>
        /// <param name="password">пароль</param>
        /// <param name="rememberMe">запомнить меня</param>
        /// <returns></returns>
        bool Authenticate(string username, string password, bool rememberMe);

        /// <summary>
        /// вход
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="rememberMe">запомнить</param>
        void Login(UserInfo user, bool rememberMe);

        /// <summary>
        /// выход
        /// </summary>
        void Logoff();

        /// <summary>
        /// генерация пароля
        /// </summary>
        /// <param name="pass">пароль</param>
        /// <param name="salt">ключ</param>
        /// <returns></returns>
        string GeneratePassword(string pass, string salt);

        /// <summary>
        /// текущий пользователь
        /// </summary>
        UserInfo CurrentUser { get; }
    }
}