using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Handles the display of the XML para tag.
	/// </summary>
	public sealed class Para : Section {
		/// <summary>
		/// Initialises a new instance of the Para class.
		/// </summary>
		/// <param name="elements">The block level elements to display in this para.</param>
		public Para(List<Block> elements) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Blocks.AddRange(elements);
		}
	}
}
