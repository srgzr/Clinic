using PolyclinicProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Models
{
    public class ModelUser
    {
        [ScaffoldColumn(false)]
        [Display(Name = "Код")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, имя")]
        [Display(Name = "Имя")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина имени от 2 до 30 символов. Исправьте, пожалуйста")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, фамилию")]
        [Display(Name = "Фамилия")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина фамилии от 2 до 30 символов. Исправьте, пожалуйста")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, отчество")]
        [Display(Name = "Отчество")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина отчества от 2 до 30 символов. Исправьте, пожалуйста")]
        public string SurName { get; set; }

        [NotMapped]
        [Display(Name = "Пользователь")]
        public string GetFullName => $"{(FirstName != string.Empty ? (FirstName) : string.Empty)} {(LastName != string.Empty ? (LastName) : string.Empty)} {(SurName != string.Empty ? (SurName) : string.Empty)}";

        [NotMapped]
        [Display(Name = "Пользователь")]
        public string Name => $"{(FirstName != string.Empty ? (FirstName) : string.Empty)} {(LastName != string.Empty ? (LastName) : string.Empty)} {(SurName != string.Empty ? (SurName) : string.Empty)}";

        [NotMapped]
        [Display(Name = "ФИО")]
        public string GetShotFullName => $"{(FirstName != string.Empty ? (FirstName) : string.Empty)} {(LastName != string.Empty ? (LastName.Remove(1).ToUpper()) : string.Empty)}. {(SurName != string.Empty ? (SurName.Remove(1).ToUpper()) : string.Empty)}.";

        [Required(ErrorMessage = "Введите, пожалуйста, дату рождения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Birthday { get; set; }

        [Display(Name = "Логин")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина логина от 6 до 20 символов. Исправьте, пожалйста")]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина пароля от 6 до 20 символов. Исправьте, пожалйста")]
        public string Password { get; set; }

        [Display(Name = "Проверка пароля")]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, номер телефона")]
        [Display(Name = "Номер телефона")]
        [StringLength(9, ErrorMessage = "Длина телефона 9 символов. Исправьте, пожалуйста")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Введите эл. почту")]
        [Display(Name = "Эл. почта")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Не корретная эл. почта")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Длина эл. почты от 8 до 40 символов. Исправьте, пожалуйста")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Роль")]
        public int RoleId { get; set; }

        [Display(Name = "Роль")]
        public virtual RoleInfo RoleInfo { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, адрес магазина")]
        [Display(Name = "Адрес")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Длина адреса от 10 до 100 символов. Исправьте, пожалуйста")]
        public string Address { get; set; }

        [Display(Name = "Статус")]
        public bool IsActiv { get; set; }

        [Display(Name = "Дата регистрации")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DateRegistration { get; set; }

        [Display(Name = "Дата последнего входа")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DateLogIn { get; set; }

        [Display(Name = "Роль")]
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}