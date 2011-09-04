using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Represents a container for details about an exception. Although this
	/// is derived from block; this is not a displayable element and is typed
	/// as such to help with the parsing. These elements should be grouped
	/// together and passed to an <see cref="ExceptionList"/>.
	/// </summary>
	public sealed class ExceptionEntry : Block {
		/// <summary>
		/// Initialises a new instance of the ExceptionEntry class.
		/// </summary>
		/// <param name="link">The hyperlink to navigate to the exception details.</param>
		/// <param name="description">The collection of parsed code elements describing the exception.</param>
		public ExceptionEntry(Hyperlink link, List<Block> description) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Link = link;
			this.Description = description;
		}

		/// <summary>
		/// Gets or sets the hyperlink that takes you to the definition of the
		/// exception.
		/// </summary>
		public Hyperlink Link { get; set; }

		/// <summary>
		/// Gets or sets the description as a collection of parsed CodeElements.
		/// </summary>
		public List<Block> Description { get; set; }
	}
}
