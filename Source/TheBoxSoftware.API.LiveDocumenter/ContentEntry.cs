
namespace TheBoxSoftware.API.LiveDocumenter
{
    using System.Collections.Generic;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Documentation;

    // The content entry class is a basic wrapper to the Entry class, it is supposed to provide
    // read only access to the entries members and children.

    /// <summary>
    /// Represents an individual entry in the <see cref="Documentation"/>.
    /// </summary>
    /// <include file='Documentation\contententry.xml' path='members/member[@name="ContentEntry"]/*'/>
    public sealed class ContentEntry
    {
        private Entry _entry;                   // the internal representation of the entry data

        internal ContentEntry(Entry entry)
        {
            _entry = entry;
        }

        /// <summary>
        /// Retrieves a list of the parent entries for this ContentEntry
        /// </summary>
        /// <returns>A sequential list of parents or an empty list if the entry has no parents.</returns>
        /// <include file='Documentation\contententry.xml' path='members/member[@name="GetParents"]/*'/>
        public List<ContentEntry> GetParents()
        {
            List<ContentEntry> parents = new List<ContentEntry>();

            ContentEntry currentParent = Parent;

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
        /// <returns>A CrefPath instance that describes this element or an empty CRefPath.</returns>
        /// <include file='Documentation\contententry.xml' path='members/member[@name="CRefPath"]/*'/>
        public CRefPath CRefPath
        {
            get 
            {
                if (_entry.Item is ReflectedMember && !(_entry.Item is AssemblyDef))  // assemblies cant be cref'd
                { 
                    ReflectedMember member = _entry.Item as ReflectedMember;
                    return CRefPath.Parse(TheBoxSoftware.Reflection.Comments.CRefPath.Create(member).ToString());
                }
                else if (_entry.Item is KeyValuePair<string, List<TypeDef>>)         // namespace
                {
                    return CRefPath.Parse(string.Format("N:{0}", ((KeyValuePair<string, List<TypeDef>>)_entry.Item).Key));
                }
                else if (_entry.Item is TheBoxSoftware.Documentation.EntryTypes)
                {
                    CRefPath path = new LiveDocumenter.CRefPath();
                    path.PathType = CRefTypes.Error;

                    TheBoxSoftware.Documentation.EntryTypes entryType = (TheBoxSoftware.Documentation.EntryTypes)_entry.Item;

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
                    return new CRefPath();
                }
            }
        }

        /// <summary>
        /// Gets the key for this documentation entry.
        /// </summary>
        public long Key
        {
            get { return _entry.Key; }
        }

        /// <summary>
        /// Gets the subkey for this documentation entry.
        /// </summary>
        public string SubKey
        {
            get { return _entry.SubKey; }
        }

        /// <summary>
        /// Gets a displayable version of the name for this documentation entry.
        /// </summary>
        public string DisplayName
        {
            get { return _entry.Name; }
        }

        /// <summary>
        /// Indicates if this type is a container entry or a member entry.
        /// </summary>
        public bool IsContainer
        {
            // note: be sure to indicate in the documentation that there is a different between a container and member
            get { return !string.IsNullOrEmpty(SubKey); }
        }

        /// <summary>
        /// Gets a reference to the collection containing a sequence of child ContentEntrys for this entry.
        /// </summary>
        public ContentEntryCollection Children
        {
            get { return new ContentEntryCollection(_entry.Children); }
        }

        /// <summary>
        /// Gets a reference to the parent entry.
        /// </summary>
        public ContentEntry Parent
        {
            get
            {
                ContentEntry entry = null;
                if (_entry.Parent != null)
                {
                    entry = new ContentEntry(_entry.Parent);
                }
                return entry;
            }
        }

        /// <summary>
        /// Indicates if the member has XML comments associated with it.
        /// </summary>
        public bool HasComments
        {
            get { return _entry.XmlCommentFile != null; }
        }

        /// <summary>
        /// Obtains a reference to the entry this content entry is wrapping.
        /// </summary>
        internal Entry Entry
        {
            get { return _entry; }
        }
    }
}
