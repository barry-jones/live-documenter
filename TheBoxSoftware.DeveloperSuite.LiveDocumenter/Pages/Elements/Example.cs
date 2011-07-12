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
		public Example(List<Block> children) {
			this.Blocks.AddRange(children);
		}
	}
}
