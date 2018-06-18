using System.ComponentModel.DataAnnotations;

namespace PolyclinicProject.WebUI.Models.Account
{
    public class LoginModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Введите, пожалуйста, логин")]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [UIHint("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите, пожалуйста, пароль")]
        public string Password { get; set; }
    }
}