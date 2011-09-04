using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Represents an example element in the current document. This relates
	/// to the example xml code comment element.
	/// </summary>
	public sealed class Example : Section {
		/// <summary>
		/// Initialises a new instance of an Example class.
		/// </summary>
		/// <param name="children">The child elements to show.</param>
		public Example(List<Block> children) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Blocks.AddRange(children);
		}
	}
}
