using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qos.xin.Common
{
    /// <summary>
    /// 接口的简介
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Desc : System.Attribute
    {
        public string _Desc;
        public Desc(string Desc)
        {
            this._Desc = Desc;
        }
    }
    /// <summary>
    /// 参数名,参数值及参数简介
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Parames : System.Attribute
    {
        public string Par;
        public string val;
        public string PDesc;
        public Parames(string Parame, string val, string PDesc)
        {
            this.Par = Parame;
            this.val = val;
            this.PDesc = PDesc;
        }

    }
    public static class InterfaceDesc
    {
        public static List<Inter> GetAllDesc()
        {
            var Interlist = new List<Inter>();
            var types = Assembly.GetCallingAssembly().GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                //取接口简介
                object[] t = types[i].GetCustomAttributes(typeof(Desc), false);
                //如果为空则下一个
                if (t == null || t.Count() <= 0) continue;
                //将第一个转换成简介对象
                Desc ud = ((Desc)t[0]);
                var Parames = new List<Parame>();
                StringBuilder sb = new StringBuilder();
                //获取所有自定义特性,包括基类的
                var p = types[i].GetCustomAttributes(typeof(Parames), true);
                //把所有特性加到特性列表里
                for (int j = p.Length - 1; j >= 0; j--)
                    Parames.Add(new Parame(((Parames)p[j]).Par, ((Parames)p[j]).val, ((Parames)p[j]).PDesc));
                //获取程序集的命名空间名
                string ns = types[i].Namespace;
                int pos = ns.LastIndexOf('.');
                //从最后一个点开始截取字符到末尾
                string Url = ns.Substring(pos + 1, ns.Length - pos - 1) + "/";
                if (Url == "Interface/") Url = "";
                Interlist.Add(new Inter(types[i].BaseType == typeof(System.Web.UI.Page) ? Url + types[i].Name + ".aspx" : Url + types[i].Name + ".ashx", ud._Desc, Parames));

            }
            return Interlist.OrderBy(t => t.Desc).ToList();
        }

    }
    public class Inter
    {
        public Inter(string Url, string Desc, List<Parame> Parameter)
        {
            this.Url = Url;
            this.Desc = Desc;
            this.Parameter = Parameter.ToList();
        }
        public string Url { get; set; }
        public string Desc { get; set; }
        public List<Parame> Parameter { get; set; }


    }
    public class Parame
    {
        public Parame(string Name, string Value, string PDesc)
        {
            this.Name = Name;
            this.Value = Value;
            this.PDesc = PDesc;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string PDesc { get; set; }

    }
}
