namespace PolyclinicProject.Domain.Entities
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class CommanEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        [Display(Name = "Код")]
        public int Id { get; set; }

        [Display(Name = "Активность")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}