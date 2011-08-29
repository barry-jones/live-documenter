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

		/// <summary>
		/// Test for a correctly defined numbered list
		/// </summary>
		/// <remarks>
		/// <list type="number">
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
		void NumberedList() { }

		/// <summary>
		/// Test for a quick ill defined numbered list (no decsription or term)
		/// </summary>
		/// <remarks>
		/// <list type="number">
		///		<item>Item one</item>
		///		<item>Item two</item>
		///		<item>Item three</item>
		///		<item>Item four</item>
		/// </list>
		/// </remarks>
		void QuickNumberedList() { }

		/// <remarks>
		/// <list type="table">
		///		<listheader>
		///			<term>Custom term title</term>
		///			<description>Custom description title</description>
		///		</listheader>
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>A description of the term.</description>
		///		</item>
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>A description of the term.</description>
		///		</item>
		/// </list>
		/// </remarks>
		/// <summary>
		/// A test to see how the application handles a correctly defined term list.
		/// </summary>
		void ACorrectTableList() { }

		/// <remarks>
		/// <list type="table">
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>A description of the term.</description>
		///		</item>
		/// </list>
		/// </remarks>
		/// <summary>
		/// A test to see how the application handles a correctly defined term list.
		/// </summary>
		void TableWithNoHeaderDefined() { }

		/// <remarks>
		/// <list type="table">
		///		<listheader>
		///			<term>Custom term title</term>
		///			<description>Custom description title</description>
		///		</listheader>
		/// </list>
		/// </remarks>
		/// <summary>
		/// A test to see how the application handles a table with no list items just a header
		/// </summary>
		void ATableWithNoListItems() { }

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

		/// <summary>
		/// A test to see how the application handles a list which has direct content and not
		/// via a description or term.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>Item one</item>
		///		<item>Item two</item>
		///		<item>Item three</item>
		///		<item>Item four</item>
		/// </list>
		/// </remarks>
		void ItemsWithContentNotDescription() { }

		/// <summary>
		/// Test a table with a sub table
		/// </summary>
		/// <remarks>
		/// <list type="table">
		///		<listheader>
		///			<term>Custom term title</term>
		///			<description>Custom description title</description>
		///		</listheader>
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>
		/// <list type="table">
		///		<listheader>
		///			<term>Custom term title</term>
		///			<description>Custom description title</description>
		///		</listheader>
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>A description of the term.</description>
		///		</item>
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>A description of the term.</description>
		///		</item>
		/// </list>
		///			</description>
		///		</item>
		///		<item>
		///			<term>A term requiring definition</term>
		///			<description>A description of the term.</description>
		///		</item>
		/// </list>
		/// </remarks>
		void TableWithChild() { }

		/// <summary>
		/// Test to see how the application handles child lists
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>
		///			<description>Item one</description>
		///		</item>
		///		<item>
		///			<description>
		/// <list type="bullet">
		///		<item>
		///			<description>Sub item one</description>
		///		</item>
		///		<item>
		///			<description>Sub item two</description>
		///		</item>
		///		<item>
		///			<description>Sub item three</description>
		///		</item>
		///		<item>
		///			<description>Sub item four</description>
		///		</item>
		/// </list>
		///			</description>
		///		</item>
		///		<item>
		///			<description>Item three</description>
		///		</item>
		///		<item>
		///			<description>Item four</description>
		///		</item>
		/// </list>
		/// </remarks>
		void ChildLists() { }

		/// <remarks>
		/// <list type="bullet">
		///		<item>
		/// <list type="bullet">
		///		<item>Sub item one</item>
		///		<item>Sub item two</item>
		///		<item>Sub item three</item>
		///		<item>Sub item four</item>
		/// </list>
		///		</item>
		///		<item>Item two</item>
		///		<item>Item three</item>
		///		<item>Item four</item>
		/// </list>
		/// </remarks>
		/// <summary>
		/// Tests how the appliction handles a quick defined list with lists children
		/// </summary>
		void QuickChildLists() { }
	}
}
