using System;
using System.Collections.Generic;
using System.ComponentModel;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation
{
    /// <include file='code-documentation\entry.xml' path='docs/entry/member[@name="entry"]/*' />
	[System.Diagnostics.DebuggerDisplay("Key: {Key} SubKey: {SubKey}")]
    public class Entry : INotifyPropertyChanged, IComparable<Entry>
    {
        private XmlCodeCommentFile _xmlComments;
        private object _item;
        private bool _isExpanded;
        private bool _isSelected;
        private bool _isSearchable;
        private string _name;
        private long _key;
        private string _subKey;
        private Entry _parent;
        private List<Entry> _children;

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[@name="ctor1"]/*' />
		public Entry(object item, string displayName, XmlCodeCommentFile xmlComments)
        {
            _item = item;
            _xmlComments = xmlComments;
            _name = displayName;
            _children = new List<Entry>();
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[@name="ctor2"]/*' />
		public Entry(object item, string displayName, XmlCodeCommentFile xmlComments, Entry parent)
            : this(item, displayName, xmlComments)
        {
            Parent = parent;
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="findbykey1"]/*' />
		public Entry FindByKey(long key, string subKey)
        {
            return FindByKey(key, subKey, true);
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="findbykey2"]/*' />
		public Entry FindByKey(long key, string subKey, bool checkChildren)
        {
            Entry found = null;

            if (IsThisEntry(key, subKey))
            {
                found = this;
            }
            else if (Children != null && checkChildren)
            {
                int count = Children.Count;
                for (int i = 0; i < count; i++)
                {
                    found = Children[i].FindByKey(key, subKey);
                    if (found != null)
                    {
                        break;
                    }
                }
            }

            return found;
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="search"]/*' />
        public List<Entry> Search(string searchText)
        {
            List<Entry> results = new List<Entry>();

            if (IsSearchable && Name.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                results.Add(this);
            }

            int count = Children.Count;
            for (int i = 0; i < count; i++)
            {
                results.AddRange(Children[i].Search(searchText));
            }

            return results;
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="findnamespace"]/*' />
		public Entry FindNamespace(string name)
        {
            // a namespace entry is an entry with a List<TypeDef> member, the required namespace should be
            // a parent of this type (provided it was a type of member of a type).
            Entry parent = this;

            while (parent.Parent != null && !(parent.Item is KeyValuePair<string, List<TypeDef>>))
            {
                parent = parent.Parent;
            }

            return parent;
        }

        private bool IsThisEntry(long key, string subKey)
        {
            return _key == key && ((string.IsNullOrEmpty(_subKey) == string.IsNullOrEmpty(subKey)) || (_subKey == subKey));
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="name"]/*' />
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// The unique key that can be used to find the entry in a document map.
        /// </summary>
        public long Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        /// <summary>
        /// A subkey for the entry which allows us to differentiate between multiple children
        /// where a non metadata related entry was created. For example this is used when
        /// creating entries for property, member and method pages.
        /// </summary>
        public string SubKey
        {
            get
            {
                return _subKey;
            }
            set
            {
                _subKey = value;
            }
        }

        /// <summary>
        /// Indicates if the entry is searchable.
        /// </summary>
        public bool IsSearchable
        {
            get
            {
                return _isSearchable;
            }
            set
            {
                _isSearchable = value;
            }
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="parent"]/*' />
		public Entry Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// The child entries for this Entry.
        /// </summary>
        public List<Entry> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

        /// <summary>
        /// The associated comment file for this entry.
        /// </summary>
        /// <remarks>
        /// Not sure about the Entry class retaining a reference
        /// to the comment file.
        /// </remarks>
        public XmlCodeCommentFile XmlCommentFile
        {
            get
            {
                return _xmlComments;
            }
        }

        /// <summary>
        /// Indicates if this entry is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != this._isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// Indicates if this Entry has its child entries expanded or not.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                if (value != this._isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && this.Parent != null)
                    Parent.IsExpanded = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }

        /// <summary>
        /// PropertyChanged event handler. Fires when an interesting property in this
        /// class has been changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notification event for interesting property changes in this class,
        /// helps to link the model to the view.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Default comparison method for entries. Compares the entries by there
        /// names.
        /// </summary>
        /// <param name="other">The other Entry to compare this one to.</param>
        /// <returns>An integer representing the results of the comparison.</returns>
        public int CompareTo(Entry other)
        {
            return _name.CompareTo(other._name);
        }
    }
}