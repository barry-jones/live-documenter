
namespace TheBoxSoftware.Documentation
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the structure and content of a document that is being
    /// used by the documentation consumers to access, search and display
    /// the documentation.
    /// </summary>
    public class DocumentMap : IList<Entry>
    {
        private readonly IList<Entry> _baseCollection;

        /// <summary>
        /// Initialises a new instance of the DocumentMap
        /// </summary>
        public DocumentMap()
            : this(new List<Entry>())
        {
        }

        /// <summary>
        /// Initialises a new instance of the DocumentMap class but allows the
        /// caller to specify the type of generic list to be used as the basis
        /// of the document map.
        /// </summary>
        /// <param name="baseCollection"></param>
        protected DocumentMap(IList<Entry> baseCollection)
        {
            _baseCollection = baseCollection;
        }

        /// <summary>
        /// Searches the entire document map for the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The globally unique id to search for.</param>
        /// <returns>The found entry or null if not found.</returns>
        public Entry FindById(long id)
        {
            Entry found = null;
            for (int i = 0; i < Count; i++)
            {
                found = this[i].FindByKey(id, string.Empty);
                if (found != null) break;
            }
            return found;
        }

        /// <summary>
        /// Sorts the top level document map entries by Entry.Name
        /// </summary>
        public void Sort()
        {
            List<Entry> temp = _baseCollection.ToList();
            temp.Sort();

            _baseCollection.Clear();
            foreach (Entry current in temp)
            {
                _baseCollection.Add(current);
            }
        }

        /// <summary>
        /// The total number of entries (including children) that are contained in this document map.
        /// </summary>
        /// <remarks>
        /// This is not a calculated property but is set after the document mapper has completed.
        /// </remarks>
        public int NumberOfEntries
        {
            get; set;
        }

        public int IndexOf(Entry item)
        {
            return _baseCollection.IndexOf(item);
        }

        public void Insert(int index, Entry item)
        {
            _baseCollection.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _baseCollection.RemoveAt(index);
        }

        public Entry this[int index]
        {
            get
            {
                return _baseCollection[index];
            }
            set
            {
                _baseCollection[index] = value;
            }
        }

        public void Add(Entry item)
        {
            _baseCollection.Add(item);
        }

        public void Clear()
        {
            _baseCollection.Clear();
        }

        public bool Contains(Entry item)
        {
            return _baseCollection.Contains(item);
        }

        public void CopyTo(Entry[] array, int arrayIndex)
        {
            _baseCollection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _baseCollection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _baseCollection.IsReadOnly;
            }
        }

        public bool Remove(Entry item)
        {
            return _baseCollection.Remove(item);
        }

        public IEnumerator<Entry> GetEnumerator()
        {
            return _baseCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _baseCollection.GetEnumerator();
        }
    }
}