using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PolyclinicProject.Domain.Entities;

namespace PolyclinicProject.WebUI.Models
{
    public class ScheduleModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "Код")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Доктор не указан")]
        [Display(Name = "Доктор")]
        public int PersonalId { get; set; }

        [Display(Name = "Доктор")]
        [Required(ErrorMessage = "Доктор не указан")]
        public Personal Personal { get; set; }
        [Display(Name = "Доктора")]
        public IEnumerable<System.Web.Mvc.SelectListItem> Personals { get; set; }
        [Range(100, 999, ErrorMessage = "Недопустимый кабинет")]
        [Display(Name = "Кабинет")]
        public int Cabinet { get; set; }

        [Display(Name = "Четное число месяца?")]
        public bool Even { get; set; }

        [Display(Name = "Первая смена?")]
        public bool IsFirstShift { get; set; }

        [Display(Name = "Активность")]
        public bool IsActive { get; set; }
    }
}