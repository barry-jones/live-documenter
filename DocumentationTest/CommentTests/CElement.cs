using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// This is a <c>test</c> of the c element.
	/// </summary>
	/// <remarks>
	/// <para>Where is a <c>c</c> element valid? In this para?</para>
	/// Or outside of any <c>c</c>?
	/// <list type="bullet">
	///		<item>In a <c>list</c> item.</item>
	/// </list>
	/// </remarks>
	class CElement {
		/// <summary>
		/// <C>Some people</C> just wont enter details correctly. <C>This is the kind of crap that occurs.</c>
		/// </summary>
		/// <remarks>
		/// This now silently fails.
		/// </remarks>
		void InvalidCasing() { }
	}
}
