using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Qos.xin.Common
{
    public static class EnumExtensions
    {
        public static List<SelectListItem> ToSelectListItem<T>(this Enum enumType)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (int s in Enum.GetValues(typeof(T)))
            {
                list.Add(new SelectListItem { Value = s.ToString(), Text = Enum.GetName(typeof(T), s) });
            }
            return list;
        }
    }
}
