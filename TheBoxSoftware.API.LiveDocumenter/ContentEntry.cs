using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Documentation;

namespace TheBoxSoftware.API.LiveDocumentor
{
    // The content entry class is a basic wrapper to the Entry class, it is supposed to provide
    // read only access to the entries members and children.

    /// <summary>
    /// Represents an individual entry in the <see cref="Documentation"/>.
    /// </summary>
    public sealed class ContentEntry
    {
        private TheBoxSoftware.Documentation.Entry entry;                   // the internal representation of the entry data

        internal ContentEntry(TheBoxSoftware.Documentation.Entry entry)
        {
            this.entry = entry;
        }

        /// <summary>
        /// Retrieves a list of the parent entries for this ContentEntry
        /// </summary>
        /// <returns>A sequential list of parents or an empty list if the entry has no parents.</returns>
        public List<ContentEntry> GetParents()
        {
            List<ContentEntry> parents = new List<ContentEntry>();

            ContentEntry currentParent = this.Parent;

            while(currentParent != null)
            {
                parents.Insert(0, currentParent);
                currentParent = currentParent.Parent;
            }

            return parents;
        }

        /// <summary>
        /// Gets the unique crefpath for this documentation entry.
        /// </summary>
        public string CRefPath
        {
            get 
            {
                if (entry.Item is ReflectedMember && !(entry.Item is AssemblyDef))  // assemblies cant be cref'd
                { 
                    ReflectedMember member = entry.Item as ReflectedMember;
                    return TheBoxSoftware.Reflection.Comments.CRefPath.Create(member).ToString();
                }
                else if (entry.Item is KeyValuePair<string, List<TypeDef>>)         // namespace
                {
                    return string.Format("N:{0}",((KeyValuePair<string, List<TypeDef>>)entry.Item).Key);
                }
                else if (entry.Item is TheBoxSoftware.Documentation.EntryTypes)
                {
                    string path = string.Empty;
                    TheBoxSoftware.Documentation.EntryTypes entryType = (TheBoxSoftware.Documentation.EntryTypes)entry.Item;

                    switch (entryType)
                    {
                        case EntryTypes.NamespaceContainer:                         // namespace contatainers do not have a crefpath??
                            break;
                        default:
                            break;
                    }

                    return path;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the key for this documentation entry.
        /// </summary>
        public long Key
        {
            get { return this.entry.Key; }
        }

        /// <summary>
        /// Gets the subkey for this documentation entry.
        /// </summary>
        public string SubKey
        {
            get { return this.entry.SubKey; }
        }

        /// <summary>
        /// Gets a displayable version of the name for this documentation entry.
        /// </summary>
        public string DisplayName
        {
            get { return this.entry.Name; }
        }

        /// <summary>
        /// Gets a reference to the collection containing a sequence of child ContentEntrys for this entry.
        /// </summary>
        public ContentEntryCollection Children
        {
            get { return new ContentEntryCollection(this.entry.Children); }
        }

        /// <summary>
        /// Gets a reference to the parent entry.
        /// </summary>
        public ContentEntry Parent
        {
            get
            {
                ContentEntry entry = null;
                if (this.entry.Parent != null)
                {
                    entry = new ContentEntry(this.entry.Parent);
                }
                return entry;
            }
        }

        /// <summary>
        /// Obtains a reference to the entry this content entry is wrapping.
        /// </summary>
        internal Entry Entry
        {
            get { return this.entry; }
        }
    }
}
