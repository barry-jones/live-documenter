using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// <para>This is some summary information, inside a paragraph.</para>
	/// This is not inside a para element and contains a <see cref="AllOutputTypesClass"/>
	/// link.
	/// <para>This is a <see cref="AllOutputTypesClass"/> inside a para; this is <c>some code.</c>
	/// <code>
	/// Somewhat incorrectly added code block element.
	/// </code>
	/// </para>
	/// <example>
	/// This is an example with a paragraph outside the para element.
	/// <para>This is inside the para element.</para>
	/// <code>
	/// This code block is inside the example block.
	/// </code>
	/// </example>
	/// This method <see cref="ATestMethod(int)" />, is simply a test. <see cref="ATestMethod(string)"/>. <see cref="Gah{T}()"/>. <see cref="Gah{T}(string)"/>
	/// </summary>
	/// <remarks>
	/// <para>Here are some remarks inside a para element.</para>
	/// <example>Apparaently examples are not allowed in remarks!
	/// <code>
	/// int i = 3;
	/// i++;
	/// </code>
	/// </example>
	/// <list>
	///		<listheader>This is the header for the list!</listheader>
	///		<item>A plain item with text</item>
	///		<item><code>string x = "testing code";</code></item>
	///		<item><para>A couple of paragraphs of information in a list.</para><para>This will look shit. Does para = paragraph or parameter??</para></item>
	/// </list>
	/// </remarks>
	/// <typeparam name="T">This is detail for the type parameter information</typeparam>
	/// <seealso cref="Object"/>
	public class Gah<T> {
		public Gah() {
		}

		public Gah(string s) {
		}

		/// <summary>
		/// Value is apparently what youwould put on a private member. <see cref="ATestMethod" />.
		/// <example>
		/// Hoorah!
		/// </example>
		/// </summary>
		private bool booleanIn;
		/// <summary>
		/// This is some summary information about a property
		/// </summary>
		public string Test { get; set; }
		
        /// <summary>
        /// Value is apparently what youwould put on a private member. <see cref="ATestMethod" />
        /// </summary>
        public Gah<string> CHild { get; set; }

		/// <summary>
		/// THis is some summary information about a method. The parameter <paramref name="i"/> is
		/// not really that interesting. But <typeparamref name="T"/> on the class has no effect on this
		/// method whatsoever!
		/// </summary>
		/// <param name="i">This is some detail about the parameter.</param>
		/// <returns>It returns a string represetnation of the integer</returns>
		/// <seealso cref="AllOutputTypesClass"/>
		public string ATestMethod(int i) {
			return i.ToString();
		}

		public string ATestMethod(string s) {
			return string.Empty;
		}
	}
}
