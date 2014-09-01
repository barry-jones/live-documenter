using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Test the uses and basterdisations of the <c>include</c> element.
	/// </summary>
	class IncludeElement {
		/// <include file='includedfile.xml' path='mydocs/member[@name="first"]/*'/>
		void IncludedFirst() { }

		/// <include file='includedfile.xml' path='mydocs/member[@name="second"]/*'/>
		void IncludedSecond() { }
	}
}
