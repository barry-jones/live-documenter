using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Tests the various uses and basterdisations of the &lt;exception&gt; element.
	/// </summary>
	public class ExceptionElement {
		/// <summary>
		/// Tests the basic uses of the exception element.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// This is a known type in the current environment and should work correctly.
		/// </exception>
		public void Basic() { throw new ArgumentNullException(); }

		/// <summary>
		/// Tests the display of multiple excetions.
		/// </summary>
		/// <exception cref="ArgumentNullException">ArgumentNullException</exception>
		/// <exception cref="ArgumentException">ArgumentException</exception>
		/// <exception cref="Exception">Exception</exception>
		public void MultipleExceptions() { }

		/// <summary>
		/// Tests that the links are created correctly to exceptions defined in the project.
		/// </summary>
		/// <exception cref="ArgumentNullException">Not defined and should not be linked.</exception>
		/// <exception cref="TestException">Defined and should be linked.</exception>
		/// <exception cref="ExceptionElement">Valid type not a valid exception.</exception>
		public void ReferencableException() { }
	}
}
