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
		/// Factory method for creating new <see cref="Entry"/> instances.
		/// </summary>
		/// <param name="item">The item the Entry is representing.</param>
		/// <param name="displayName">The display name of the Entry.</param>
		/// <param name="comments">The XmlCodeCommentFile associated with the <paramref name="item"/>s assembly.</param>
		/// <returns>The created Entry</returns>
		public override Entry Create(object item, string displayName, Reflection.Comments.XmlCodeCommentFile comments) {
			return new LiveDocumenterEntry(item, displayName, comments);
		}

		/// <summary>
		/// Factory method for creating new <see cref="Entry"/> instances.
		/// </summary>
		/// <param name="item">The item the Entry is representing.</param>
		/// <param name="displayName">The display name of the Entry.</param>
		/// <param name="comments">The XmlCodeCommentFile associated with the <paramref name="item"/>s assembly.</param>
		/// <param name="parent">The parent for this Entry</param>
		/// <returns>The created Entry</returns>
		public override Entry Create(object item, string displayName, Reflection.Comments.XmlCodeCommentFile comments, Entry parent) {
			return new LiveDocumenterEntry(item, displayName, comments, parent);
		}
	}
}
