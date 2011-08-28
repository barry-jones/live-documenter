using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// Represents a key for an entry. The key is a unique identifier for an element
	/// which is unique across all properties, methods, parameters etc in a library.
	/// </summary>
	public class EntryKey {
		/// <summary>
		/// Initialises a new instance of the EntryKey class.
		/// </summary>
		protected EntryKey() { }

		/// <summary>
		/// Initialises a new instance of the EntryKey class.
		/// </summary>
		/// <param name="key">The key for the entry.</param>
		public EntryKey(long key) { this.Key = key; }

		/// <summary>
		/// Initialises a new instance of the EntryKey class.
		/// </summary>
		/// <param name="key">The key for the entry.</param>
		/// <param name="subKey">The subkey for the entry.</param>
		public EntryKey(long key, string subKey) {
			this.Key = key;
			this.SubKey = subKey;
		}

		/// <summary>
		/// The unique key.
		/// </summary>
		public long Key;

		/// <summary>
		/// The unique subkey.
		/// </summary>
		public string SubKey;
	}

	/// <summary>
	/// Represents a key for an entry that is a unique reference to point in another
	/// assembly.
	/// </summary>
	public class CrefEntryKey : EntryKey {
		/// <summary>
		/// Initialises a new instance of the CrefEntryKey class.
		/// </summary>
		/// <param name="assembly">The assembly the cref points to.</param>
		/// <param name="cref">The cref path to the entry.</param>
		public CrefEntryKey(AssemblyDef assembly, string cref) {
			this.Assembly = assembly;
			this.CRef = cref;

			// NOTE: we are fixing crappy names back to correct cref formats.
			if (cref.Contains('<')) {
				MatchCollection matches = Regex.Matches(cref, "<[a-zA-Z](,[a-zA-Z])*>");
				for (int i = 0; i < matches.Count; i++) {
					string replacement = string.Format("`{0}", matches[i].Value.Split(',').Length);
					cref = cref.Replace(matches[i].Value, replacement);
				}
				this.CRef = cref;
			}
		}

		/// <summary>
		/// The assembly the cref points to.
		/// </summary>
		public AssemblyDef Assembly;
		
		/// <summary>
		/// The CRef describing the entry.
		/// </summary>
		public string CRef;
	}
}
