using PolyclinicProject.Domain.Abstract.ListItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyclinicProject.Domain.Entities
{
    public sealed class Personal : CommanEntity, ISelectListItem
    {
        public Personal()
        {
            Schedules = new HashSet<Schedule>();
        }

        public ICollection<Schedule> Schedules { get; set; }

        [Display(Name = "Пользователь")]
        public int UserInfoId { get; set; }

        [Display(Name = "Пользователь")]
        public UserInfo UserInfo { get; set; }

        [Display(Name = "Поликлиника")]
        public int PolyclinicId { get; set; }

        [Display(Name = "Поликлиника")]
        public Polyclinic Polyclinic { get; set; }

        [Display(Name = "Должность")]
        public int PositionId { get; set; }

        [Display(Name = "Должность")]
        public Position Position { get; set; }

        [NotMapped]
        [Display(Name = "Пользователь")]
        public string Name
        {
            get { return  $"{Polyclinic?.Name}, {Position?.Name}, {UserInfo?.GetShotFullName}"; }
            set { }
        }
    }
}