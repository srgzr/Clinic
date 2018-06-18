using System.ComponentModel.DataAnnotations;

namespace PolyclinicProject.WebUI.Models
{
    public class PositionModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "Код")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, наименование должности")]
        [Display(Name = "Наименование")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина наименования от 2 до 50 символов. Исправьте, пожалуйста")]
        public string Name { get; set; }

        [Display(Name = "Статус")]
        public bool IsActive { get; set; }
    }
}