using PolyclinicProject.Domain.Abstract.ListItem;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PolyclinicProject.Domain.Extensions
{
    /// <summary>
    /// работа с выпадающим списком
    /// </summary>
    public static class SelectListExtensions
    {
        public static IList<SelectListItem> GetPageSize(this IList<SelectListItem> items)
        {
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "15", Value = "15" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            return items;
        }

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> items, bool addEmptyItem = true) where T : ISelectListItem
        {
            var selectListElements = items.Where(s => s.IsActive).Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }).ToList();
            if (addEmptyItem)
                selectListElements.Insert(0, new SelectListItem { Text = "", Value = "" });
            return selectListElements;
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<int> items, bool addEmptyItem = true)
        {
            var selectListElements = items.Select(p => new SelectListItem { Text = p.ToString(), Value = p.ToString() }).ToList();
            if (addEmptyItem)
                selectListElements.Insert(0, new SelectListItem { Text = "", Value = "" });
            return selectListElements;
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<string> items, bool addEmptyItem = true)
        {
            var selectListElements = items.Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            if (addEmptyItem)
                selectListElements.Insert(0, new SelectListItem { Text = "", Value = "" });
            return selectListElements;
        }
    }
}