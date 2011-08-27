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
	/// <example>
	/// This is an example inside a remark!
	/// <code>
	/// DocumentedClass c = new DocumentedClass();
	/// </code>
	/// </example>
	/// </remarks>
	/// <example>
	/// You use the documented class like this.
	/// <code>
	/// DocumentedClass doc = new DocumentedClass();
	/// doc.GenericClassMethod&lt;string&gt;("Hello world.");
	/// </code>
	/// </example>
	public class DocumentedClass {
		/// <summary>
		/// Constructor documentation
		/// </summary>
		public DocumentedClass() {
		}

		/// <field>
		/// Fiels is not an acknoledged element.
		/// </field>
		/// <summary>This is the summary for the field</summary>
		/// <value>It contains an integer.</value>
		/// <remarks>These are remarks for the field</remarks>
		/// <example>
		/// Nothing really to have an example of but just testing.
		/// <code>
		/// public DocumentedClass() {
		///   this.aField = 3;
		/// }
		/// </code>
		/// </example>
		private int aField;

		/// <summary>
		/// Documentation for a class method
		/// </summary>
		/// <param name="aString">The parameter</param>
		/// <returns>Returns something</returns>
		/// <example>
		/// It is called like this.
		/// <code>
		/// DocumentedClass c = new DocumentedClass();
		/// string[] myStrings = new string[] { "One", "Two" };
		/// d.DoSomething(ref myStrings);
		/// </code>
		/// </example>
		/// <exception cref="ArgumentException">Prob with the <paramref name="aString"/> argument.</exception>
		/// <exception cref="InvalidOperationException">The operation was invalid.</exception>
		public int DoSomething(ref string[] aString) {
			return 0;
		}

		/// <summary>
		/// A generic methods documentation with a constraint
		/// </summary>
		/// <typeparam name="T">The type t</typeparam>
		/// <param name="something">The parameter</param>
		/// <exception cref="ArgumentException">Prob with the <paramref name="aString"/> argument.</exception>
		/// <exception cref="InvalidOperationException">The operation was invalid.</exception>
		public void GenericClassMethod<T>(T something) where T: class {
		}

		/// <summary>
		/// Documentation for a class property.
		/// </summary>
		/// <remarks>
		/// The name property has lots of awesome things about it.
		/// <para>They are too awesome <b>to</b> explain.</para>
		/// </remarks>
		/// <example>
		/// This is an example of a property.
		/// <code>
		/// DocumentedClass c = new DocumentedClass();
		/// c.Name = "Joe Bloggs";
		/// </code>
		/// </example>
		/// <exception cref="ArgumentException">Prob with the <paramref name="aString"/> argument.</exception>
		/// <exception cref="InvalidOperationException">The operation was invalid.</exception>
		public string Name { get; set; }

		/// <returns>Returns a boolean</returns>
		/// <param name="param1">The first parameter</param>
		/// <summary>
		/// A summary of the method
		/// </summary>
		/// <param name="param2">The second parameter</param>
		/// <exception cref="ArgumentException">Prob with the <paramref name="aString"/> argument.</exception>
		/// <exception cref="InvalidOperationException">The operation was invalid.</exception>
		public bool OutOfOrderDocumentedMethod(string param1, string param2) {
			return true;
		}
	}
}
