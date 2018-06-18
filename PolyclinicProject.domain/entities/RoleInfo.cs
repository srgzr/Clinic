using PolyclinicProject.Domain.Abstract.ListItem;
using System.ComponentModel.DataAnnotations;

namespace PolyclinicProject.Domain.Entities
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    /// <summary>
    /// роли
    /// </summary>
    public class RoleInfo : IdentityRole<int, AppUserRole>, ISelectListItem
    {
        public RoleInfo()
        {
            UserInfo = new HashSet<UserInfo>();
        }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Пользователи")]
        public ICollection<UserInfo> UserInfo { get; set; }

        [Display(Name = "Активность")]
        public bool IsActive { get; set; } = true;
    }
}