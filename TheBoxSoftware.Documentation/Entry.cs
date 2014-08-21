using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// Class that represents an entry in the live document, relates to a single page
	/// so defines the details of a method, type assembly and diagram pages etc. It
	/// also contains information to populate the document tree in the user interface.
	/// </summary>
    /// <remarks>
    /// <para>
    /// Keys are unique et
    /// </para>
    /// </remarks>
	[System.Diagnostics.DebuggerDisplay("Key: {Key} SubKey: {SubKey}")]
	public class Entry : INotifyPropertyChanged, IComparable<Entry> {
		private XmlCodeCommentFile xmlComments;
		private object item;
		private bool isExpanded;
		private bool isSelected;
        private bool isSearchable;
		private string name;

		#region Constructors
		/// <summary>
		/// Initialises a new Entry instance
		/// </summary>
		/// <param name="item">The item that represents this entry</param>
		/// <param name="xmlComments">The XmlComments file</param>
		public Entry(object item, string displayName, XmlCodeCommentFile xmlComments) {
			this.item = item;
			this.xmlComments = xmlComments;
			this.Name = displayName;
			this.Children = new List<Entry>();
		}

		/// <summary>
		/// Initialises a new instance of the Entry class.
		/// </summary>
		/// <param name="item">The item associated with the entry.</param>
		/// <param name="displayName">The display name of the entry.</param>
		/// <param name="xmlComments">The xml comments file for the assembly.</param>
		/// <param name="parent">The parent node.</param>
		public Entry(object item, string displayName, XmlCodeCommentFile xmlComments, Entry parent)
			: this(item, displayName, xmlComments) {
			this.Parent = parent;
		}
		#endregion

		/// <summary>
		/// Iterates over the complete document map and attempts to find
		/// the item specified by the key.
		/// </summary>
		/// <param name="key">The key to search the document map for</param>
		/// <returns>The found keyvalue pair or null if not found.</returns>
		public Entry FindByKey(long key, string subKey) {
			return this.FindByKey(key, subKey, true);
		}

		/// <summary>
		/// Iterates over the complete document map and attempts to find
		/// the item specified by the key.
		/// </summary>
		/// <param name="key">The key to search the document map for</param>
		/// <returns>The found keyvalue pair or null if not found.</returns>
		public Entry FindByKey(long key, string subKey, bool checkChildren) {
			Entry found = null;

			if (this.Key == key && string.IsNullOrEmpty(subKey)) {
				found = this;
			}
			else if (this.Key == key && this.SubKey == subKey) {
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

		/// <summary>
		/// Search this entry and its children for the specified text. This will
		/// search the full name of all <see cref="IsSearchable"/> entries and its
		/// children.
		/// </summary>
		/// <param name="searchText">The text to search for.</param>
		/// <returns>An array of entries that match the criteria.</returns>
		public List<Entry> Search(string searchText) {
			List<Entry> results = new List<Entry>();
			if (this.IsSearchable && this.Name.ToLower().Contains(searchText.ToLower())) {
				results.Add(this);
			}
			foreach (Entry child in this.Children) {
				results.AddRange(child.Search(searchText));
			}
			return results;
		}

		/// <summary>
		/// Finds the namespace in the document map based on the name provided.
		/// </summary>
		/// <param name="name">The fully qualified namespace name.</param>
		/// <returns>The found namespace entry or null if not found.</returns>
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
		/// <summary>
		/// The display name for this entry, this will be used foremost to
		/// display in the DocumentMap for the LiceDocument. This does not
		/// have to be unique.
		/// </summary>
		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		/// <summary>
		/// The unique key that can be used to find the entry in a document map.
		/// </summary>
		public long Key { get; set; }

		/// <summary>
		/// A subkey for the entry which allows us to differentiate between multiple children
		/// where a non metadata related entry was created. For example this is used when
		/// creating entries for property, member and method pages.
		/// </summary>
		public string SubKey { get; set; }

		/// <summary>
		/// Indicates if the entry is searchable.
		/// </summary>
		public bool IsSearchable {
            get { return this.isSearchable; }
            set { this.isSearchable = value; }
        }

		/// <summary>
		/// The parent entry for this Entry
		/// </summary>
		public Entry Parent { get; set; }

		/// <summary>
		/// The child entries for this Entry.
		/// </summary>
		public List<Entry> Children { get; set; }

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
