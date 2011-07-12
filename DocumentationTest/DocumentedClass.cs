using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// This is documentation for a class. <see cref="DocumentationTest"/>
	/// </summary>
	/// <remarks>
	/// It is a test to output general information about a class. Some <i>italic</i>,
	/// <b>bold</b>.
	/// </remarks>
	public class DocumentedClass {
		/// <summary>
		/// Constructor documentation
		/// </summary>
		public DocumentedClass() {
		}

		/// <field>
		/// Documentation for a field
		/// </field>
		private int aField;

		/// <summary>
		/// Documentation for a class method
		/// </summary>
		/// <param name="aString">The parameter</param>
		/// <returns>Returns something</returns>
		public int DoSomething(ref string[] aString) {
			return 0;
		}

		/// <summary>
		/// A generic methods documentation with a constraint
		/// </summary>
		/// <typeparam name="T">The type t</typeparam>
		/// <param name="something">The parameter</param>
		public void GenericClassMethod<T>(T something) where T: class {
		}

		/// <summary>
		/// Documentation for a class property.
		/// </summary>
		public string Name { get; set; }

		/// <returns>Returns a boolean</returns>
		/// <param name="param1">The first parameter</param>
		/// <summary>
		/// A summary of the method
		/// </summary>
		/// <param name="param2">The second parameter</param>
		public bool OutOfOrderDocumentedMethod(string param1, string param2) {
			return true;
		}
	}
}
