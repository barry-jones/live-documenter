using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.CommentTests {
	/// <summary>
	/// Tests the various uses and basterdisations of the example XML element.
	/// </summary>
	class ExampleElement {
		/// <summary>
		/// Tests the exmample element as a child of a remark then as a child of a paragraph.
		/// </summary>
		/// <remarks>
		/// <example>
		/// This example is a child of the remarks element.
		/// List<PerformanceReviewSummary> reviewDetails = this.facade.GetReviews();
		/// </example>
		/// <para>
		/// This para element has an example.
		/// <example>
		/// This is example is the child of a para element.
		/// </example>
		/// Even if that doesnt necessarily make sense.
		/// </para>
		/// </remarks>
		void AsAChildElemement() { }

		/// <summary>
		/// This test examples defined at the top level.
		/// </summary>
		/// <example>
		/// This is an example, the first example.
		/// List&lt;PerformanceReviewSummary> reviewDetails = this.facade.GetReviews();
		/// </example>
		void AsATopLevelElement() { }

		/// <summary>
		/// Two examples at a top level is not normally allowed.
		/// </summary>
		/// <example>
		/// Example one!
		/// List&lt;PerformanceReviewSummary&gt; reviewDetails = this.facade.GetReviews();
		/// </example>
		/// <example>
		/// Example two!
		/// </example>
		void TwoTopLevelExamples() { }
	}
}
