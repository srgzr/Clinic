using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyclinicProject.WebUI.Models
{
    public class ListModel<T>
    {
        public List<T> InfoModelForMont { get; set; }
        public List<T> InfoModelForWeek { get; set; }
        public List<T> InfoModelNow { get; set; }
    }

}