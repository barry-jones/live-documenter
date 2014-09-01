using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// See <see cref="Property"/> and see <see cref="Field1"/>.
	/// <para>
	/// Test seeing a class <see cref="DocumentedClass"/>. External,
	/// <see cref="TheBoxSoftware.Reflection.ReflectedMember"/> and finally
	/// outside the solution <see cref="System.Object"/>.
	/// </para>
	/// <para>Namespace: <see cref="DocumentationTest"/></para>
	/// </summary>
	public class SeeDocumentationTest {
		public string Field1;
		public string Field2;
		public string Property { get; set; }
	}
}
