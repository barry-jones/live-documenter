using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	class ReferenceInterfaceDocumentationTest : InterfaceTest {
		/// <summary>
		/// Implements <see cref="InterfaceTest.Method"/>
		/// </summary>
		public void Method() {
		}

		/// <summary>
		/// Implements <see cref="InterfaceTest.Property"/>
		/// </summary>
		public string Property {
			get { return string.Empty; }
			set { }
		}
	}
}
