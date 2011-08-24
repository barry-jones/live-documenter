using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// <list type="">
	/// <listheader>Header</listheader>
	///		<item>This is the first list item</item>
	///			<item>Random<list type="">
	///				<item>This list has a list!</item>
	///			</list></item>
	/// </list>
	/// </summary>
	/// <remarks>
	/// <list>
	///		<term>A term</term>
	///		<term>Another term</term>
	///		<description>Are these supposed to be 1-1?</description>
	///		<description>A second description with no term.</description>
	/// </list>
	/// </remarks>
	class ListElement {
		/// <summary>
		/// <list type="">
		///     <unsupported></unsupported>
		///		<listheader>Header</listheader>
		///		<item>This is the first list item</item>
		///			<item>Random<list type="">
		///				<item>This list has a list!</item>
		///			</list></item>
		/// </list>
		/// </summary>
		void Method() { }

		/// <remarks>
		/// <list type="term">
		///		<listheader>
		///			<term>Term</term>
		///			<description>Description</description>
		///		</listheader>
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>A description of the term.</description>
		///		</item>
		/// </list>
		/// </remarks>
		/// <summary>
		/// A test to see how the application handles a correctly defined term list.
		/// </summary>
		void ACorrectTermList() { }

		/// <summary>
		/// A test to see how the application handles a correctly defined bullet list.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>
		///			<description>Item one</description>
		///		</item>
		///		<item>
		///			<description>Item two</description>
		///		</item>
		///		<item>
		///			<description>Item three</description>
		///		</item>
		///		<item>
		///			<description>Item four</description>
		///		</item>
		/// </list>
		/// </remarks>
		void ACorrectBulletList() { }
	}
}
