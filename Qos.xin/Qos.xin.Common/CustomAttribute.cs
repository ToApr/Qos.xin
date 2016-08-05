using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qos.xin.Common
{

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class IUAttribute : Attribute
    {
        readonly FlagType _IUType;

        public IUAttribute(FlagType IUType)
        {
            this._IUType = IUType;

        }

        public FlagType IUType
        {
            get { return _IUType; }
        }

        public int NamedInt { get; set; }
        public enum FlagType { NoInsert, NoUpdate }
    }
}
