using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// Event arguments information detailing entries being added to the document map.
	/// </summary>
	public sealed class PreEntryAddedEventArgs : EventArgs {
		/// <summary>
		/// Initialises a new instance of the PreEntryAddedEventArgs class.
		/// </summary>
		/// <param name="member">The member being added to the document map.</param>
		public PreEntryAddedEventArgs(ReflectedMember member) {
			this.Member = member;
		}

		/// <summary>
		/// The member being added to the documentation map.
		/// </summary>
		public ReflectedMember Member { get; set; }

		/// <summary>
		/// A boolean value which tells the <see cref="DocumentMapper"/> to filter or not
		/// the <see cref="ReflectedMember"/>.
		/// </summary>
		public bool Filter { get; set; }
	}
}
