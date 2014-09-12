using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace v4._0.v4
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// An extension method in .net using dynamics
        /// </summary>
        /// <param name="self"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static IEnumerable<string> WhereEntityIs(this IEnumerable<string> self, params string[] metadata)
        {
            return new List<string>();
        }
    }
}
