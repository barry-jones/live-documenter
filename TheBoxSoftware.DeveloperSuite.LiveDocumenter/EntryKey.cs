using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// Represents a key for an entry. The key is a unique identifier for an element
	/// which is unique across all properties, methods, parameters etc in a library.
	/// </summary>
	public class EntryKey {
		protected EntryKey() { }
		public EntryKey(long key) { this.Key = key; }
		public EntryKey(long key, string subKey) {
			this.Key = key;
			this.SubKey = subKey;
		}

		public long Key;
		public string SubKey;
	}

	/// <summary>
	/// Represents a key for an entry that is a unique reference to point in another
	/// assembly.
	/// </summary>
	public class CrefEntryKey : EntryKey {
		public CrefEntryKey(AssemblyDef assembly, string cref) {
			this.Assembly = assembly;
			this.CRef = cref;
		}
		public AssemblyDef Assembly;
		public string CRef;
	}
}
