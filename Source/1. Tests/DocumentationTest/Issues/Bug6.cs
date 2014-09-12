using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues
{
    /// <summary>
    /// Test bug 6: Exception thrown when an extension method is defined on a type that does
    /// not exist in the project libraries.
    /// </summary>
    public static class Bug6
    {
        // code to throw error
        /// <summary>
        /// A summary for the WhereEntityIs extension method - will never be seen.
        /// </summary>
        /// <param name="self">Us</param>
        /// <param name="metadata">Some info.</param>
        /// <returns>An enumerable collection</returns>
        public static IEnumerable<string> WhereEntityIs(this IEnumerable<string> self, params string[] metadata)
        {
            return new List<string>();
        }
    }
}
