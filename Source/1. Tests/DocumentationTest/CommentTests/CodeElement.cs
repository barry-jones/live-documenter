using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Tests the various uses and basterdisations of the &lt;code&gt; element.
	/// </summary>
	public class CodeElement {
		/// <summary>
		/// Here is a code element in the summary tag.
		/// <code>
		/// Line 1: int x = 0;
		/// Line 2: x++;
		/// Line 3:
		/// Line 4: x.ToString();
		/// </code>
		/// </summary>
		/// <remarks>
		/// These are some remarks with code.
		/// <code>
		/// Line 1: int x = 0;
		/// Line 2: x++;
		/// Line 3:
		/// Line 4: x.ToString();
		/// </code>
		/// </remarks>
		/// <example>
		/// This is an example with code.
		/// <code>
		/// Line 1: int x = 0;
		/// Line 2: x++;
		/// Line 3:
		/// Line 4: x.ToString();
		/// </code>
		/// </example>
		public void AsAChildElement() { }

		/// <summary>As a top level element this should not be displayed.</summary>
		/// <code>
		/// Line 1: int x = 0;
		/// Line 2: x++;
		/// Line 3:
		/// Line 4: x.ToString();
		/// </code>
		public void TopLevelElement() { }
	}
}
