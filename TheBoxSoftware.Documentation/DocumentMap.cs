using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation
{
	public class DocumentMap : IList<Entry>
	{
		private IList<Entry> baseCollection;

		/// <summary>
		/// Initialises a new instance of the DocumentMap
		/// </summary>
		public DocumentMap()
			: this(new List<Entry>()) {
		}

		protected DocumentMap(IList<Entry> baseCollection) {
			this.baseCollection = baseCollection;
		}

		#region IList<Entry> Members

		public int IndexOf(Entry item)
		{
			return this.baseCollection.IndexOf(item);
		}

		public void Insert(int index, Entry item)
		{
			this.baseCollection.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.baseCollection.RemoveAt(index);
		}

		public Entry this[int index]
		{
			get
			{
				return this.baseCollection[index];
			}
			set
			{
				this.baseCollection[index] = value;
			}
		}

		#endregion

		#region ICollection<Entry> Members

		public void Add(Entry item)
		{
			this.baseCollection.Add(item);
		}

		public void Clear()
		{
			this.baseCollection.Clear();
		}

		public bool Contains(Entry item)
		{
			return this.baseCollection.Contains(item);
		}

		public void CopyTo(Entry[] array, int arrayIndex)
		{
			this.baseCollection.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this.baseCollection.Count; }
		}

		public bool IsReadOnly
		{
			get { return this.baseCollection.IsReadOnly; }
		}

		public bool Remove(Entry item)
		{
			return this.baseCollection.Remove(item);
		}

		#endregion

		#region IEnumerable<Entry> Members

		public IEnumerator<Entry> GetEnumerator()
		{
			return this.baseCollection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.baseCollection.GetEnumerator();
		}

		#endregion
	}
}
