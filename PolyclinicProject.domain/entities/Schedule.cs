using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PolyclinicProject.Domain.Abstract.ListItem;

namespace PolyclinicProject.Domain.Entities
{
    public class Schedule : CommanEntity, ISelectListItem
    {
        [Required(ErrorMessage = "Доктор не указан")]
        [Display(Name = "Доктор")]
        public int PersonalId { get; set; }

        [Display(Name = "Доктор")]
        [Required(ErrorMessage = "Доктор не указан")]
        public Personal Personal { get; set; }

        [Range(100, 999, ErrorMessage = "Недопустимый кабинет")]
        [Display(Name = "Кабинет")]
        public int Cabinet { get; set; }

        [Display(Name = "Четное число месяца?")]
        public bool Even { get; set; }

        [Display(Name = "Первая смена?")]
        public bool IsFirstShift { get; set; }

        [NotMapped]
        public string Name
        {
            get { return $"{Personal?.Name}, Кабинет: {Cabinet}, четный день: { (Even?"ДА":"Нет") }, смена {(IsFirstShift ? "Первая" : "Вторая")}"; }
            set { }
        }
    }
}
