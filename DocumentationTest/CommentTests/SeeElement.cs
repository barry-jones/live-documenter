using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	///	This class tests various uses and basterdisations of the see element.
	/// </summary>
	/// <remarks>
	/// The <see cref="SeeElement"/> can be used to link to other elements.
	/// </remarks>
	class SeeElement {
		/// <summary>
		/// This tests the casing of the element and its attributes.
		/// </summary>
		/// <remarks>
		/// We can <see cref="InvalidCasing"/>! <SEE cref="InvalidCasing"/> and <see CREF="InvalidCasing"/>.
		/// </remarks>
		void InvalidCasing() { }
	}
}
