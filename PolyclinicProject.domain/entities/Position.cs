using PolyclinicProject.Domain.Abstract.ListItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PolyclinicProject.Domain.Entities
{
    public sealed class Position : CommanEntity, ISelectListItem
    {
        public Position()
        {
            Personals = new HashSet<Personal>();
        }

        [Required(ErrorMessage = "Введите, пожалуйста, наименование должности")]
        [Display(Name = "Наименование")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина наименования от 2 до 50 символов. Исправьте, пожалуйста")]
        public string Name { get; set; }

        [Display(Name = "Сотрудники")]
        public ICollection<Personal> Personals { get; set; }
    }
}