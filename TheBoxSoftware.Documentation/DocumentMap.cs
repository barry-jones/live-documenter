using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	/// <summary>
	/// Represents the structure and content of a document that is being
	/// used by the documentation consumers to access, search and display
	/// the documentation.
	/// </summary>
	public class DocumentMap : IList<Entry> {
		private IList<Entry> baseCollection;

		/// <summary>
		/// Initialises a new instance of the DocumentMap
		/// </summary>
		public DocumentMap()
			: this(new List<Entry>()) {
		}

		/// <summary>
		/// Initialises a new instance of the DocumentMap class but allows the
		/// caller to specify the type of generic list to be used as the basis
		/// of the document map.
		/// </summary>
		/// <param name="baseCollection"></param>
		protected DocumentMap(IList<Entry> baseCollection) {
			this.baseCollection = baseCollection;
		}

		/// <summary>
		/// Searches the entire document map for the provided <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The globally unique id to search for.</param>
		/// <returns>The found entry or null if not found.</returns>
		public Entry FindById(long id) {
			Entry found = null;
			for (int i = 0; i < this.Count; i++) {
				found = this[i].FindByKey(id, string.Empty);
				if (found != null) break;
			}
			return found;
		}

		/// <summary>
		/// Sorts the top level document map entries by Entry.Name
		/// </summary>
		public void Sort() {
			List<Entry> temp = this.baseCollection.ToList();
			temp.Sort();

			this.baseCollection.Clear();
			foreach(Entry current in temp) {
				this.baseCollection.Add(current);
			}
		}

		/// <summary>
		/// The total number of entries (including children) that are contained in this document map.
		/// </summary>
		/// <remarks>
		/// This is not a calculated property but is set after the document mapper has completed.
		/// </remarks>
		public int NumberOfEntries { get; set; }

		#region IList<Entry> Members

		public int IndexOf(Entry item) {
			return this.baseCollection.IndexOf(item);
		}

		public void Insert(int index, Entry item) {
			this.baseCollection.Insert(index, item);
		}

		public void RemoveAt(int index) {
			this.baseCollection.RemoveAt(index);
		}

		public Entry this[int index] {
			get {
				return this.baseCollection[index];
			}
			set {
				this.baseCollection[index] = value;
			}
		}

		#endregion

		#region ICollection<Entry> Members

		public void Add(Entry item) {
			this.baseCollection.Add(item);
		}

		public void Clear() {
			this.baseCollection.Clear();
		}

		public bool Contains(Entry item) {
			return this.baseCollection.Contains(item);
		}

		public void CopyTo(Entry[] array, int arrayIndex) {
			this.baseCollection.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return this.baseCollection.Count; }
		}

		public bool IsReadOnly {
			get { return this.baseCollection.IsReadOnly; }
		}

		public bool Remove(Entry item) {
			return this.baseCollection.Remove(item);
		}

		#endregion

		#region IEnumerable<Entry> Members

		public IEnumerator<Entry> GetEnumerator() {
			return this.baseCollection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.baseCollection.GetEnumerator();
		}

		#endregion
	}
}
