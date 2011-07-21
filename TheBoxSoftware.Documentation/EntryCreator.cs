using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// A class that controls the creation of <see cref="Entry"/> instances and subtypes for
	/// a <see cref="DocumentMap"/>.
	/// </summary>
	public class EntryCreator {
		public virtual Entry Create(object item, string displayName, XmlCodeCommentFile comments) {
			return new Entry(item, displayName, comments);
		}

		public virtual Entry Create(object item, string displayName, XmlCodeCommentFile comments, Entry parent) {
			return new Entry(item, displayName, comments, parent);
		}
	}
}
