using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PolyclinicProject.Domain.Abstract.ListItem;

namespace PolyclinicProject.Domain.Entities
{
    public sealed class Polyclinic :  CommanEntity, ISelectListItem
    {
        public Polyclinic()
        {
            Personals = new HashSet<Personal>();
        }

        [Required(ErrorMessage = "Введите, пожалуйста, наименование магазина")]
        [Display(Name = "Наименование")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина наименования от 2 до 30 символов. Исправьте, пожалуйста")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, адрес магазина")]
        [Display(Name = "Адрес")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Длина адреса от 10 до 100 символов. Исправьте, пожалуйста")]
        public string Address { get; set; }

        [Display(Name = "Сотрудники")]
        public ICollection<Personal> Personals { get; set; }
    }
}
