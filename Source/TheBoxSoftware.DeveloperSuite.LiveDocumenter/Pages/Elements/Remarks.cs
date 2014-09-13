using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Represents some remarks about an element in the current document. This
	/// refers to the remarks xml code comments element.
	/// </summary>
	public sealed class Remarks : Section {
		/// <summary>
		/// Initialises a new instance of the Remarks class
		/// </summary>
		/// <param name="children">The child elements of the section</param>
		public Remarks(List<Block> children) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Blocks.Add(new Header2("Remarks"));
			this.Blocks.AddRange(children);
		}
	}
}
