using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Documentation;
using System.Collections;

namespace TheBoxSoftware.API.LiveDocumenter
{
    /// <summary>
    /// Represents a sequence of ContentEntry elements.
    /// </summary>
    public sealed class ContentEntryCollection : IEnumerable
    {
        private List<Entry> entries;

        internal ContentEntryCollection(List<Entry> entries)
        {
            this.entries = entries;
        }

        /// <summary>
        /// Gets the ContentEntry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the ContentEntry to return.</param>
        /// <returns>The ContentEntry at the specified index.</returns>
        public ContentEntry this[int index]
        {
            get
            {
                return new ContentEntry(this.entries[index]);
            }
        }

        /// <summary>
        /// Returns the number of top level elements in this <see cref="Documentation"/>.
        /// </summary>
        public int Count
        {
            get { return this.entries.Count; }
        }

        /// <summary>
        /// Indicates if the collection is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)new Enumerator(this.entries);
        }

        private class Enumerator : IEnumerator
        {
            private List<Entry> entries;
            private int position = -1;

            internal Enumerator(List<Entry> entries)
            {
                this.entries = entries;
            }

            object IEnumerator.Current
            {
                get
                {
                    return new ContentEntry(this.entries[this.position]);
                }
            }

            bool IEnumerator.MoveNext()
            {
                this.position++;
                return this.position < this.entries.Count;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }

    }
}
