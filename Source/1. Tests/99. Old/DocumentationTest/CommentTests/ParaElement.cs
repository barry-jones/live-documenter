using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests
{
    /// <summary>
    /// Tests various implementation so the &lt;para> element.
    /// </summary>
    public class ParaElement
    {
        /// <summary>
        /// An empty and redundant para element.
        /// </summary>
        /// <para></para>
        public void TestEmptyPara() { }

        /// <summary>
        /// A para element with only a single space ' ' in it.
        /// </summary>
        /// <remarks>
        /// <para> </para>
        /// </remarks>
        public void TestParaWithOneSpace() { }
    }
}
