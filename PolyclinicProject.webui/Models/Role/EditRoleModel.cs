using System.ComponentModel.DataAnnotations;

namespace PolyclinicProject.WebUI.Models.Role
{
    public class EditRoleModel
    {
        [Display(Name = "Код роли")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите, пожалуйста, наименование роли")]
        [Display(Name = "Роль")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина наименования роли от 2 до 50 символов. Исправьте, пожалуйста")]
        public string Name { get; set; }

        [Display(Name = "Описание роли")]
        public string Description { get; set; }

        [Display(Name = "Состояние")]
        public bool IsActive { get; set; }
    }
}