using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Represents the entire documentation for a set of assemblies.
	/// </summary>
	public class Document {
        // 16 bytes
        private DocumentMapper mapper;
        private DocumentSettings settings;
        private DocumentMap map;
        private List<DocumentedAssembly> assemblies;

		#region Constructors
		/// <summary>
		/// Initialises a new instance of the Document class.
		/// </summary>
		/// <param name="assemblies">The assemblies being documented.</param>
		public Document(List<DocumentedAssembly> assemblies)
			: this(assemblies, Mappers.GroupedNamespaceFirst, false, new EntryCreator()) {
		}

		/// <summary>
		/// Initialises a new instance of the Document class.
		/// </summary>
		/// <param name="assemblies">The assemblies being documented.</param>
		/// <param name="mapperType">The type of document mapper to use to create the document map</param>
		/// <param name="useObservableCollection">Should the document map use an observable collection.</param>
		/// <param name="creator">The EntryCreator to use to create new Entries in the Map</param>
		public Document(List<DocumentedAssembly> assemblies, Mappers mapperType, bool useObservableCollection, EntryCreator creator) {
			this.Mapper = DocumentMapper.Create(assemblies, mapperType, useObservableCollection, creator);
			this.Mapper.PreEntryAdded += new EventHandler<PreEntryAddedEventArgs>(PreEntryAdded);

			this.Assemblies = assemblies;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The DocumentMapper used to generate the DocumentMap
		/// </summary>
		protected DocumentMapper Mapper {
            get { return this.mapper; }
            set { this.mapper = value; }
        }

		/// <summary>
		/// The settings for this document.
		/// </summary>
		public DocumentSettings Settings {
            get { return this.settings; }
            set { this.settings = value; }
        }

		/// <summary>
		/// The generated DocumentMap
		/// </summary>
		public DocumentMap Map {
            get { return this.map; }
            set { this.map = value; }
        }

		/// <summary>
		/// The assemblies being documented.
		/// </summary>
		public List<DocumentedAssembly> Assemblies {
            get { return this.assemblies; }
            set { this.assemblies = value; }
        }

		/// <summary>
		/// Indicates if this document has <see cref="Assemblies"/>.
		/// </summary>
		public bool HasFiles {
			get { return this.Assemblies != null && this.Assemblies.Count > 0; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Updates the document map based on the current <see cref="Assemblies"/> and <see cref="Settings"/>.
		/// </summary>
		public void UpdateDocumentMap() {
			this.Mapper.GenerateMap();
			this.Map = this.Mapper.DocumentMap;
		}

		/// <summary>
		/// Indicates if the member should be filtered from the document.
		/// </summary>
		/// <param name="member">The member to test.</param>
		/// <returns>True if it should be filtered otherwise false.</returns>
		/// <remarks>
		/// This can be used to quickly test wether a member should be in the document before attempting
		/// to search the entire <see cref="DocumentMap"/>.
		/// </remarks>
		public bool IsMemberFiltered(ReflectedMember member) {
			bool shouldBeAdded = true;

			// test the visibility of the member
			if (shouldBeAdded &&
				(member is MethodDef ||
				member is PropertyDef ||
				member is FieldDef ||
				member is TypeDef ||
				member is EventDef)) {

                // we always show public member, but if this member is not public test against
                // the other displayable filters
				bool publicVisibility = member.MemberAccess == Visibility.Public;
				if (!publicVisibility) {
					shouldBeAdded = false;
					foreach (Visibility current in this.Settings.VisibilityFilters) {
						if (member.MemberAccess == current) {
							shouldBeAdded = true;
							break;
						}
					}
				}
			}

			return !shouldBeAdded;
		}

		/// <summary>
		/// Searches the entire document map for the specified <paramref name="key"/> and <paramref name="subkey"/>.
		/// </summary>
		/// <param name="key">The unique key.</param>
		/// <param name="subkey">The unqiue subkey</param>
		/// <returns>The Entry if found else null.</returns>
		public Entry Find(long key, string subkey) {
			Entry found = null;
			for (int i = 0; i < this.Map.Count; i++) {
				found = this.Map[i].FindByKey(key, subkey);
				if (found != null) break;
			}
			return found;
		}

		/// <summary>
		/// Searches the Document for the entry related to the specified <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The CRefPath to search for.</param>
		/// <returns>The Entry if it is found else null.</returns>
		public Entry Find(CRefPath path) {
			if(path == null || path.PathType == CRefTypes.Error) return null;	// nothing to search for
			if (this.Map.Count == 0) return null; // nothing to search in

			// find the level of the namespace entries
			int level = 1;
			Entry found = this.Map[0];
			while (!(found.Item is KeyValuePair<string, List<TypeDef>>)) {
				found = found.Children[0];
				level++;
			}
			found = null;

			List<Entry> namespaceEntries = new List<Entry>();	// flattend list of namespaces
			if (level == 1) {
				namespaceEntries.AddRange(this.Map);
			}
			else {
				for (int i = 0; i < this.Map.Count; i++) {
					namespaceEntries.AddRange(this.GetAllEntriesFromLevel(level, 2, this.Map[i]));
				}
			}

			for (int i = 0; i < namespaceEntries.Count; i++) {
				Entry currentLevel = namespaceEntries[i];
				if (string.Compare(currentLevel.SubKey, path.Namespace, true) == 0) {
					// if we are searching for a namespace, we are done now as we have found it
					if (path.PathType == CRefTypes.Namespace) {
						found = currentLevel;
						break;
					}

					// search the types in the namespace
					for (int j = 0; j < currentLevel.Children.Count; j++) {
						Entry currentTypeEntry = currentLevel.Children[j];
						TypeDef currentType = (TypeDef)currentTypeEntry.Item;
						if (string.Compare(currentType.Name, path.TypeName) == 0) {
							// if we are searchinf for a type, we are done now
							if (path.PathType == CRefTypes.Type) {
								found = currentTypeEntry;
								break;
							}

							// find the type and do the quick type search to find the related
							// entry. this is the quickest way.
							ReflectedMember member = path.FindIn(currentType);
							if (member != null) {	// someone could have misspelled the member in the crefpath
								found = currentTypeEntry.FindByKey(member.GetGloballyUniqueId(), string.Empty);
								break;
							}
						}
					}
				}
				if (found != null) break;
			}

			return found;
		}

		/// <summary>
		/// Searches a tree and returns all of the entries at a specific level.
		/// </summary>
		/// <param name="fromLevel">The level to return elements from.</param>
		/// <param name="currentLevel">The current level we are at.</param>
		/// <param name="fromEntry">The entry to get entries from.</param>
		/// <returns>A flattened list of all entries at <paramref name="fromLevel"/>.</returns>
		private List<Entry> GetAllEntriesFromLevel(int fromLevel, int currentLevel, Entry fromEntry) {
			if (currentLevel > fromLevel) return new List<Entry>();

			if (fromLevel == currentLevel) {
				return fromEntry.Children;
			}
			else {
				List<Entry> collectedEntries = new List<Entry>();
				for (int i = 0; i < fromEntry.Children.Count; i++) {
					collectedEntries.AddRange(this.GetAllEntriesFromLevel(fromLevel, currentLevel + 1, fromEntry.Children[i]));
				}
				return collectedEntries;
			}
		}

		/// <summary>
		/// Searches the entire document tree and returns all elements that match the search
		/// <paramref name="criteria"/>.
		/// </summary>
		/// <param name="criteria">The search criteria.</param>
		/// <returns>The found entries.</returns>
		public List<Entry> Search(string criteria) {
			List<Entry> results = new List<Entry>();
			if (this.Map != null) {
				for (int i = 0; i < this.Map.Count; i++) {
					results.AddRange(this.Map[i].Search(criteria));
				}
			}
			return results;
		}
		#endregion

		#region Event Handlers
		/// <summary>
		/// Method that handles the PreEntryAdded event fired by the DocumentMapper before a
		/// member is added to the document map being generated.
		/// </summary>
		/// <param name="sender">The calling object.</param>
		/// <param name="e">The event arguments.</param>
		private void PreEntryAdded(object sender, PreEntryAddedEventArgs e) {
			e.Filter = this.IsMemberFiltered(e.Member);
		}
		#endregion
	}
}
