using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Tests the various uses and basterdisations of the value element.
	/// </summary>
	public class ValueElement {
		/// <summary>
		/// Tests the use of the value element on a field.
		/// </summary>
		/// <value>Value for a field.</value>
		public string OnAFieldTest;

		/// <summary>
		/// Tests the use of the value element on a property.
		/// </summary>
		/// <value>Value for the property.</value>
		public string OnAProperty { get; set; }
	}
}
