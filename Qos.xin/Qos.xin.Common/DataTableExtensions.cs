using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qos.xin.Common
{
    public static class DataTableExtensions
    {
        public static IEnumerable<T> Convert<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo PI in propertys)
                {
                    if (dt.Columns.Contains(PI.Name))
                    {
                        if (!PI.CanWrite) continue;
                        object value = dr[PI.Name];
                        if (value != DBNull.Value)
                        {
                            PI.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list.AsEnumerable();
        }
    }
}
