using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Reflection;

    /// <include file='code-documentation\entry.xml' path='docs/entry/member[@name="entry"]/*' />
	[System.Diagnostics.DebuggerDisplay("Key: {Key} SubKey: {SubKey}")]
	public class Entry : INotifyPropertyChanged, IComparable<Entry> {
        // 38 bytes
		private XmlCodeCommentFile xmlComments;
		private object item;
		private bool isExpanded;
		private bool isSelected;
        private bool isSearchable;
		private string name;
        private long key;
        private string subKey;
        private Entry parent;
        private List<Entry> children;

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[@name="ctor1"]/*' />
		public Entry(object item, string displayName, XmlCodeCommentFile xmlComments) {
			this.item = item;
			this.xmlComments = xmlComments;
			this.Name = displayName;
			this.Children = new List<Entry>();
		}

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[@name="ctor2"]/*' />
		public Entry(object item, string displayName, XmlCodeCommentFile xmlComments, Entry parent)
			: this(item, displayName, xmlComments) {
			this.Parent = parent;
		}

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="findbykey1"]/*' />
		public Entry FindByKey(long key, string subKey) {
			return this.FindByKey(key, subKey, true);
		}

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="findbykey2"]/*' />
		public Entry FindByKey(long key, string subKey, bool checkChildren) {
			Entry found = null;

            // make sure they have the same key and sub key
            if (this.Key == key && ((string.IsNullOrEmpty(this.SubKey) == string.IsNullOrEmpty(subKey)) || (this.SubKey == subKey))) {
                found = this;
            }
			else if (this.Children != null && checkChildren) {
				int count = this.Children.Count;
				for(int i = 0; i < count; i++) {
					found = this.Children[i].FindByKey(key, subKey);
					if (found != null) {
						break;
					}
				}
			}

			return found;
		}

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="search"]/*' />
		public List<Entry> Search(string searchText) {
			List<Entry> results = new List<Entry>();

			if (this.IsSearchable && this.Name.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0) {
				results.Add(this);
			}

            int count = this.Children.Count;
            for(int i = 0; i < count; i++) {
                results.AddRange(this.Children[i].Search(searchText));
            }

			return results;
		}

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="findnamespace"]/*' />
		public Entry FindNamespace(string name) {
			// a namespace entry is an entry with a List<TypeDef> member, the required namespace should be
			// a parent of this type (provided it was a type of member of a type).
			Entry parent = this;

			while (parent.Parent != null && !(parent.Item is KeyValuePair<string, List<TypeDef>>)) {
				parent = parent.Parent;
			}

			return parent;
		}

		#region Properties
        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="name"]/*' />
		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		/// <summary>
		/// The unique key that can be used to find the entry in a document map.
		/// </summary>
		public long Key {
            get { return this.key; }
            set { this.key = value; }
        }

		/// <summary>
		/// A subkey for the entry which allows us to differentiate between multiple children
		/// where a non metadata related entry was created. For example this is used when
		/// creating entries for property, member and method pages.
		/// </summary>
		public string SubKey {
            get { return this.subKey; }
            set { this.subKey = value; }
        }

		/// <summary>
		/// Indicates if the entry is searchable.
		/// </summary>
		public bool IsSearchable {
            get { return this.isSearchable; }
            set { this.isSearchable = value; }
        }

        /// <include file='code-documentation\entry.xml' path='docs/entry/member[name="parent"]/*' />
		public Entry Parent {
            get { return this.parent; }
            set { this.parent = value; }
        }

		/// <summary>
		/// The child entries for this Entry.
		/// </summary>
		public List<Entry> Children {
            get { return this.children; }
            set { this.children = value; }
        }

		/// <summary>
		/// The associated comment file for this entry.
		/// </summary>
		/// <remarks>
		/// Not sure about the Entry class retaining a reference
		/// to the comment file.
		/// </remarks>
		public XmlCodeCommentFile XmlCommentFile { 
            get { 
                return this.xmlComments; 
            } 
        }

		/// <summary>
		/// Indicates if this entry is selected.
		/// </summary>
		public bool IsSelected {
			get { return this.isSelected; }
			set {
				if (value != this.isSelected) {
					this.isSelected = value;
					this.OnPropertyChanged("IsSelected");
				}
			}
		}

		/// <summary>
		/// Indicates if this Entry has its child entries expanded or not.
		/// </summary>
		public bool IsExpanded {
			get {
				return this.isExpanded;
			}
			set {
				if (value != this.isExpanded) {
					this.isExpanded = value;
					this.OnPropertyChanged("IsExpanded");
				}

				// Expand all the way up to the root.
				if (this.isExpanded && this.Parent != null)
					this.Parent.IsExpanded = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public object Item { 
			get { return this.item; }
			set { this.item = value; }
		}
		#endregion

		#region INotifyPropertyChanged Members
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
		private void OnPropertyChanged(string propertyName) {
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		#region IComparable<Entry> Members
		/// <summary>
		/// Default comparison method for entries. Compares the entries by there
		/// names.
		/// </summary>
		/// <param name="other">The other Entry to compare this one to.</param>
		/// <returns>An integer representing the results of the comparison.</returns>
		public int CompareTo(Entry other) {
			return this.name.CompareTo(other.name);
		}
		#endregion
	}
}
