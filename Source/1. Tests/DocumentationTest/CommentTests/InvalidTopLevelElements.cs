using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// This class has an invalid top level XML element
	/// </summary>
	/// <invalid>
	/// <para>It does have some content!</para>
	/// </invalid>
	class InvalidTopLevelElements {
		/// <nothing></nothing>
		void Method(object parameter) {
		}

		/// <testelement>This is a test</testelement>
		bool Property { get; set; }

		/// <nv>Nothing value</nv>
		bool field;
	}
}
