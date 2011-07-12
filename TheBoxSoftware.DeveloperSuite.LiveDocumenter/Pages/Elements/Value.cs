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
		public Value(List<Block> blocks) {
			this.Blocks.Add(new Header2("Summary"));
			this.Blocks.AddRange(blocks);
		}
	}
}
