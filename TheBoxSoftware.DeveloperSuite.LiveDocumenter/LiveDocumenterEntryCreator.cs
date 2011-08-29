using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Implementation of an EntryCreator factory.
	/// </summary>
	internal class LiveDocumenterEntryCreator : EntryCreator {
		/// <summary>
		/// Method called by the EntryCreator factory method to initialise new Entry instances.
		/// </summary>
		/// <param name="item">The item the Entry is representing.</param>
		/// <param name="displayName">The display name of the Entry.</param>
		/// <param name="comments">The XmlCodeCommentFile associated with the <paramref name="item"/>s assembly.</param>
		/// <param name="parent">The parent for this Entry</param>
		/// <returns>The created Entry</returns>
		protected override Entry InitialiseEntry(object item, string displayName, Reflection.Comments.XmlCodeCommentFile comments, Entry parent) {
			return new LiveDocumenterEntry(item, displayName, comments, parent);
		}
	}
}
