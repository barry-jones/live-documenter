using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// This class tests the implementation and support for the code element.
	/// </summary>
	public class CodeElement {
		/// <summary>
		/// This should not be displayed
		/// </summary>
		/// <code>Some code</code>
		public void TopLevelCodeElement() { }

		/// <summary>
		/// <code>
		/// Summary code
		/// </code>
		/// </summary>
		/// <remarks>
		/// <code>
		/// Remarks code
		/// </code>
		/// </remarks>
		/// <example>
		/// <code>
		/// Example code
		/// </code>
		/// </example>
		public void ChildOf() { }

		/// <summary>
		/// <para>
		/// Para with code:
		/// <code>
		/// Code in para
		/// </code>
		/// </para>
		/// </summary>
		public void InPara() { }
	}
}
