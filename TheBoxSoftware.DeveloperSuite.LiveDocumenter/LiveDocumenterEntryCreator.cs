using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Documentation;

	internal class LiveDocumenterEntryCreator : EntryCreator {
		public override Entry Create(object item, string displayName, Reflection.Comments.XmlCodeCommentFile comments) {
			return new LiveDocumenterEntry(item, displayName, comments);
		}

		public override Entry Create(object item, string displayName, Reflection.Comments.XmlCodeCommentFile comments, Entry parent) {
			return new LiveDocumenterEntry(item, displayName, comments, parent);
		}
	}
}
