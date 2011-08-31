using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// This class tests summary elements, there many guises and basterdisations.
	/// </summary>
	public class SummaryElement {
		/// <summary>
		/// This is a summary element on a constructor.
		/// </summary>
		public SummaryElement() { }

		/// <summary>
		/// This tests a summary on a property
		/// </summary>
		public string SummaryOnProperty { get; set; }

		/// <summary>
		/// This tests a summary on a field.
		/// </summary>
		public string SummaryOnAField;

		/// <summary>
		/// This is a test of a summary on a method.
		/// </summary>
		public void SummaryOnAMethod() { }

		/// <SUMMARY>
		/// This is a test of a summary with invalid casing.
		/// </SUMMARY>
		public void InvalidCasing() { }

		/// <summary>
		/// Tests multiple instances of the summary element
		/// </summary>
		/// <summary>
		/// This is the second instance.
		/// </summary>
		public void MultipleInstances() { }

		/// <summary>
		/// Test a summary as a child of a summary.
		/// <summary>
		/// People will do really weird things!
		/// </summary>
		/// </summary>
		public void SummaryAsAChildOfSummary() { }
	}
}
