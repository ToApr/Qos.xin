using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace Qos.xin.Common
{
    public class JsonBase
    {
        private Info _info = new Info();
        public JsonBase() { }
        public JsonBase(bool status, Info info, object result)
        {
            this.status = status;
            this.info = info;
            this.result = result;
        }
        public JsonBase(bool status, object result)
        {
            this.status = status;
            this.result = result;
        }
        public bool status { get; set; }

        public Info info { get { return _info; } set { _info = value as Info; } }

        public object result { get; set; }
    }
    public class Info
    {
        public string Msg { get; set; }

        public string Code { get; set; }
    }
    public static class Extensions
    {
        public static string ToJson(this JsonBase jb)
        {
            var t =new  JsonSerializerSettings(){NullValueHandling= NullValueHandling.Include, DateParseHandling=DateParseHandling.None};
            return JsonConvert.SerializeObject(jb,t);
        }
    }
}
