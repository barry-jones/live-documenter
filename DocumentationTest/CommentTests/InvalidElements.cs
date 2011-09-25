using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// <b>Bold</b> is supported. <author>Is not supported.</author> and has some text after.
	/// </summary>
	class InvalidElements {
		/// <summary>
		/// <job><code>Test code</code> is <c>Woop!</c></job> is the business.
		/// </summary>
		void Method() { }
	}
}
