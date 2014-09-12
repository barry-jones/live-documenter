using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Refers to a value, i.e. field. VS.NET normally adds summary elements
	/// to fields when declaring the xml comments. This should be changed to
	/// the value element. This refers to the value xml code comment element.
	/// </summary>
	public sealed class Value : Section {
		/// <summary>
		/// Initialises a new instance of the Value section.
		/// </summary>
		/// <param name="blocks">The child block elements to add.</param>
		public Value(List<Block> blocks) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Blocks.Add(new Header3("Value"));
			this.Blocks.AddRange(blocks);
		}
	}
}
