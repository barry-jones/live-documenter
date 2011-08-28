using System;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// A class that controls the creation of <see cref="Entry"/> instances and subtypes for
	/// a <see cref="DocumentMap"/>.
	/// </summary>
	public class EntryCreator {
		/// <summary>
		/// Creats a new Entry instance with the provided details.
		/// </summary>
		/// <param name="item">The item the Entry related to</param>
		/// <param name="displayName">The display name for the entry</param>
		/// <param name="comments">The XmlCodeComments associated with the entry.</param>
		/// <returns>A new Entry describing the <paramref name="item"/></returns>
		public virtual Entry Create(object item, string displayName, XmlCodeCommentFile comments) {
			return new Entry(item, displayName, comments);
		}

		/// <summary>
		/// Creats a new Entry instance with the provided details.
		/// </summary>
		/// <param name="item">The item the Entry related to</param>
		/// <param name="displayName">The display name for the entry</param>
		/// <param name="comments">The XmlCodeComments associated with the entry.</param>
		/// <param name="parent">The parent entry for the new Entry.</param>
		/// <returns>A new Entry describing the <paramref name="item"/></returns>
		public virtual Entry Create(object item, string displayName, XmlCodeCommentFile comments, Entry parent) {
			return new Entry(item, displayName, comments, parent);
		}
	}
}
