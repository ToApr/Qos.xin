using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qos.xin.Common
{
    /// <summary>
    /// 接口的简介
    /// </summary>
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
}
