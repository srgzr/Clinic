using System.ComponentModel.DataAnnotations;

namespace PolyclinicProject.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите, пожалуйста, логин пользователя")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, пароль пользователя")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}