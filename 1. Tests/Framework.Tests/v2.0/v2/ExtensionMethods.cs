using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
    [AttributeUsageAttribute(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class ExtensionAttribute : Attribute
    {
    }
}

namespace v2._0.v2
{

    public static class ExtensionMethods
    {
        /// <summary>
        /// A .net 2 runtime extension method
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ExentsionMethod(this string str)
        {
            return string.Empty;
        }
    }
}
