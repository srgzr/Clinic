using System.ComponentModel.DataAnnotations;

namespace PolyclinicProject.Domain.Abstract.ListItem
{
    /// <summary>
    /// Сервис для работы с выпадающим списком
    /// </summary>
    public interface ISelectListItem
    {
        /// <summary>
        /// Id
        /// </summary>
        [Display(Name = "Номер")]
        int Id { set; get; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Display(Name = "Наименование")]
        string Name { set; get; }

        [Display(Name = "Активность")]
        bool IsActive { get; set; }
    }

    public interface ISelectListItemCount
    {
        /// <summary>
        /// Id
        /// </summary>
        [Display(Name = "Номер")]
        int Id { set; get; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Display(Name = "Наименование")]
        string Name { set; get; }

        [Display(Name = "Активность")]
        bool IsActive { get; set; }

        [Display(Name = "Количество")]
        int Count { get; set; }
    }
}