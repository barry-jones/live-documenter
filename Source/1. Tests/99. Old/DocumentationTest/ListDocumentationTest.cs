using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest
{
    /// <summary>
    /// This is a test of the description of the list xml code comment element.
    /// </summary>
    public sealed class ListDocumentationTest
    {
        /// <summary>
        /// Test of - as defined by microsoft - bulletted list.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>This is the first entry.</term>
        ///     </item>
        ///     <item>
        ///         <term>This is the second element.</term>
        ///     </item>
        /// </list>
        /// </remarks>
        public void SpecBullet()
        {
        }

        /// <summary>
        /// Test of - as defined by microsoft - numbered list.
        /// </summary>
        /// <remarks>
        /// <list type="number">
        ///     <item>
        ///         <term>This is the first entry.</term>
        ///     </item>
        ///     <item>
        ///         <term>This is the second element.</term>
        ///     </item>
        /// </list>
        /// </remarks>
        public void SpecNumber()
        {
        }

        /// <summary>
        /// Test of - as defined by microsoft - table.
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Header term</term>
        ///         <description>Header description</description>
        ///     </listheader>
        ///     <item>
        ///         <term>First row term</term>
        ///         <description>First row description</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public void SpecTable() { }

        /// <summary>
        /// Test of list defined with no items.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// </list>
        /// </remarks>
        public void TestNoItems() { }

        /// <summary>
        /// Test of a simple definition (no term) and no type attribute.
        /// </summary>
        /// <remarks>
        /// <list>
        ///     <item>First</item>
        ///     <item>Second</item>
        /// </list>
        /// </remarks>
        public void SimpleNoType() { }

        /// <summary>
        /// Tests a list as a table but no header element defined.
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///     <item>
        ///         <term>First row</term>
        ///         <description>First row description</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public void TableNoHeader() { }

        /// <summary>
        /// Tests table definiton with just items - no term or description specified
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///     <listheader>
        ///         First
        ///     </listheader>
        ///     <item>
        ///         Second
        ///     </item>
        /// </list>
        /// </remarks>
        public void TableNoTermOrDescription() { }
    }
}
