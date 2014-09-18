using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Documentation;
using System.Collections;
using System.Xml;

namespace TheBoxSoftware.API.LiveDocumenter {

    /// <summary>
    /// Provides access to the table of contents for <see cref="Documentation"/>.
    /// </summary>
    /// <include file='Documentation\tableofcontents.xml' path='members/member[@name="tableofcontents"]/*'/>
    public sealed class TableOfContents : IEnumerable {
        private Document document;

        // initialises the toc class with the map reference.. this whole class will have
        // to be invalidated when the documentation is reloaded. <HOW?>
        internal TableOfContents(Document document) {
            this.document = document;
        }

        /// <summary>
        /// Retrieves the ContentEntry for the provided <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The unique Entry.Key to retrieve the documentation for.</param>
        /// <param name="subKey">The Entry.SubKey if required to get the documentation for.</param>
        /// <returns>The ContentEntry for the specified Key, or null if not found.</returns>
        /// <include file='Documentation\tableofcontents.xml' path='members/member[@name="GetEntryFor.key"]/*'/>
        public ContentEntry GetEntryFor(long key, string subKey)
        {
            Entry found = this.document.Find(key, subKey);
            return found == null ? null : new ContentEntry(found);
        }

        /// <summary>
        /// Retrieves the ContentEntry for the provided <paramref name="crefPath"/>.
        /// </summary>
        /// <param name="crefPath">The CRefPath to retrieve the documentation for.</param>
        /// <returns>The ContentEntry for the specified crefpath or null if not found.</returns>
        /// <include file='Documentation\tableofcontents.xml' path='members/member[@name="GetEntryFor.cref"]/*'/>
        public ContentEntry GetEntryFor(string crefPath)
        {
            Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Parse(crefPath);
            if (path.PathType == Reflection.Comments.CRefTypes.Error)
                throw new DocumentationException("The provided cref path {0} did not parse correctly.");

            Entry found = this.document.Find(path);
            return found == null ? null : new ContentEntry(this.document.Find(path));
        }

        /// <summary>
        /// Searches the documentation and returns a list of ContentEntrys that match the search term.
        /// </summary>
        /// <param name="text">The text to search for.</param>
        /// <returns>A list of ContentEntrys or an empty list if none are found.</returns>
        public List<ContentEntry> Search(string text)
        {
            List<Entry> results = this.document.Search(text);
            List<ContentEntry> entries = new List<ContentEntry>();

            for (int i = 0; i < results.Count; i++)
            {
                entries.Add(new ContentEntry(results[i]));
            }

            return entries;
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
                return new ContentEntry(this.document.Map[index]);
            }
        }

        /// <summary>
        /// Returns the number of top level elements in this <see cref="Documentation"/>.
        /// </summary>
        public int Count
        {
            get { return this.document.Map.Count; }
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
            return (IEnumerator)new Enumerator(this.document.Map);
        }

        private class Enumerator : IEnumerator
        {
            private DocumentMap map;
            private int position = -1;

            internal Enumerator(DocumentMap map)
            {
                this.map = map;
            }

            object IEnumerator.Current
            {
                get
                {
                    return new ContentEntry(this.map[this.position]);
                }
            }

            bool IEnumerator.MoveNext()
            {
                this.position++;
                return this.position < this.map.Count;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
