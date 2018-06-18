using PolyclinicProject.Domain.Abstract.ListItem;

namespace PolyclinicProject.Domain.Entities
{
    using Enum;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;

    public class AppUserLogin : IdentityUserLogin<int> { }

    public class AppUserRole : IdentityUserRole<int> { }

    public class AppUserClaim : IdentityUserClaim<int> { }

    public class AppClaimsPrincipal : ClaimsPrincipal
    {
        public AppClaimsPrincipal(ClaimsPrincipal principal) : base(principal)
        { }

        public int UserId => int.Parse(this.FindFirst(ClaimTypes.Sid).Value);
    }

    /// <summary>
    /// пользователь
    /// </summary>
    public sealed partial class UserInfo : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>, IUser<int>, ISelectListItem
    {
        public UserInfo()
        {
            Personals = new HashSet<Personal>();
        }

        [Display(Name = "Сотрудники")]
        public ICollection<Personal> Personals { get; set; }

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[ScaffoldColumn(false)]
        //[Display(Name = "Код пользователя")]
        //public int Id { get; set; }

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
        public string Name
        {
            get
            {
                return
                    $"{(FirstName != string.Empty ? (FirstName) : string.Empty)} {(LastName != string.Empty ? (LastName) : string.Empty)} {(SurName != string.Empty ? (SurName) : string.Empty)}";
            }
            set { }
        }

        [NotMapped]
        [Display(Name = "ФИО")]
        public string GetShotFullName => $"{(FirstName != string.Empty ? (FirstName) : string.Empty)} {(LastName != string.Empty ? (LastName.Remove(1).ToUpper()) : string.Empty)}. {(SurName != string.Empty ? (SurName.Remove(1).ToUpper()) : string.Empty)}.";

        [Display(Name = "Пол")]
        public Sex Sex { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, дату рождения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Пароль")]
        [UIHint("Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина пароля от 6 до 20 символов. Исправьте, пожалйста")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, номер телефона")]
        [Display(Name = "Номер телефона")]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{2})[-. ]?([0-9]{3})$", ErrorMessage = "Не верный формат.")]
        [StringLength(9, ErrorMessage = "Длина телефона 9 символов. Исправьте, пожалуйста")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Роль")]
        public int RoleInfoId { get; set; }

        [Display(Name = "Роль")]
        public RoleInfo RoleInfo { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, адрес магазина")]
        [Display(Name = "Адрес")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Длина адреса от 10 до 100 символов. Исправьте, пожалуйста")]
        public string Address { get; set; }

        [Display(Name = "Дата регистрации")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DateRegistration { get; set; }

        [Display(Name = "Дата последнего входа")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DateLogIn { get; set; }

        [Display(Name = "Активность")]
        public bool IsActive { get; set; } = true;
    }
}